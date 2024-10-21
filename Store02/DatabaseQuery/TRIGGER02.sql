USE STORE02


--A USAR
CREATE TRIGGER UpdateStockOnOrder
ON OrderPurchaseDetail
AFTER INSERT
AS
BEGIN

	-- Aumentar el stock si es una orden de compra ('Purchase') y est� 'Completed'
	IF EXISTS (
		SELECT 1
		FROM inserted i
		INNER JOIN OrderPurchase op ON i.OrderPID = op.OrderPID
		WHERE i.OrderPID = op.OrderPID AND op.StatusOrder = 'Completed'
	)

	BEGIN
		UPDATE p
		SET p.Stock = p.Stock + i.Quantity
		FROM Products p
		INNER JOIN inserted i ON p.ProductID = i.ProductID
		INNER JOIN OrderPurchase op ON i.OrderPID = op.OrderPID
		WHERE op.StatusOrder = 'Completed';
	END
END;

DROP TRIGGER UpdateStockOnOrder


--A USAR
CREATE TRIGGER UpdateStockOnOrderSale
ON OrderSaleDetail
AFTER INSERT
AS
BEGIN
    -- Disminuir stock si es una orden de venta ('Sale') y est� 'Pending' o 'Completed'
    IF EXISTS (SELECT 1 
               FROM inserted i 
               INNER JOIN OrderSale os ON i.OrderSID = os.OrderSID 
               WHERE os.StatusOrder = 'Pending' OR os.StatusOrder = 'Completed')
    BEGIN
        UPDATE Products
        SET Stock = Stock - i.Quantity
        FROM Products p
        INNER JOIN inserted i ON p.ProductID = i.ProductID
        INNER JOIN OrderSale os ON i.OrderSID = os.OrderSID
        WHERE os.StatusOrder = 'Pending' OR os.StatusOrder = 'Completed'
          AND p.Stock >= i.Quantity; -- Verificar que haya suficiente stock
    END
END;


DROP TRIGGER UpdateStockOnOrderSale


--A USAR
CREATE TRIGGER UpdateStockOnStatusChangePurchase
ON OrderPurchase
AFTER UPDATE
AS
BEGIN
    -- Verificar si el nuevo estado es 'Canceled' y antes era 'Completed' (para compras)
    IF EXISTS (SELECT 1 
               FROM inserted i 
               JOIN deleted d ON i.OrderPID = d.OrderPID
               WHERE i.StatusOrder = 'Canceled' AND d.StatusOrder = 'Completed')
    BEGIN
        UPDATE Products
        SET Stock = Stock - od.Quantity
        FROM Products p
        INNER JOIN OrderPurchaseDetail od ON p.ProductID = od.ProductID
        INNER JOIN inserted i ON i.OrderPID = od.OrderPID
        INNER JOIN deleted d ON d.OrderPID = i.OrderPID
        WHERE d.StatusOrder = 'Completed';
    END
END;


DROP TRIGGER UpdateStockOnStatusChangePurchase


--A USAR
CREATE TRIGGER UpdateStockOnStatusChangeSale
ON OrderSale
AFTER UPDATE
AS
BEGIN
    -- Verificar si el nuevo estado es 'Canceled' y antes era 'Pending' o 'Completed' (para ventas)
    IF EXISTS (SELECT 1 
               FROM inserted i 
               JOIN deleted d ON i.OrderSID = d.OrderSID
               WHERE i.StatusOrder = 'Canceled' AND (d.StatusOrder = 'Pending' OR d.StatusOrder = 'Completed'))
    BEGIN
        UPDATE Products
        SET Stock = Stock + od.Quantity
        FROM Products p
        INNER JOIN OrderSaleDetail od ON p.ProductID = od.ProductID
        INNER JOIN inserted i ON i.OrderSID = od.OrderSID
        INNER JOIN deleted d ON d.OrderSID = i.OrderSID
        WHERE (d.StatusOrder = 'Pending' OR d.StatusOrder = 'Completed');
    END
END;


DROP TRIGGER UpdateStockOnStatusChangeSale


--A USAR
CREATE TRIGGER InsertOrderSaleHistory
ON OrderSale
AFTER INSERT
AS
BEGIN
    INSERT INTO OrderHistory (OrderID, OrderDate, TotalAmount, StatusOrder, OrderType, ProductID, Quantity, Price)
    SELECT 
        i.OrderSID,
        i.OrderDate,
        i.TotalAmount,
        i.StatusOrder,
        'Sale', -- Tipo de orden
        d.ProductID,
        d.Quantity,
        d.Price
    FROM 
        inserted i
    JOIN 
        OrderSaleDetail d ON i.OrderSID = d.OrderSID; -- Asumiendo que tienes una tabla OrderSaleDetail
END;


--A USAR
CREATE TRIGGER InsertOrderPurchaseHistory
ON OrderPurchase
AFTER INSERT
AS
BEGIN
    INSERT INTO OrderHistory (OrderID, OrderDate, TotalAmount, StatusOrder, OrderType, ProductID, Quantity, Price)
    SELECT 
        i.OrderPID,
        i.OrderDate,
        i.TotalAmount,
        i.StatusOrder,
        'Purchase', -- Tipo de orden
        d.ProductID,
        d.Quantity,
        d.Price
    FROM 
        inserted i
    JOIN 
        OrderPurchaseDetail d ON i.OrderPID = d.OrderPID; -- Asumiendo que tienes una tabla OrderPurchaseDetail
END;


--A USAR
CREATE TRIGGER UpdateOrderHistoryOnPurchase
ON OrderPurchase
AFTER UPDATE
AS
BEGIN
    INSERT INTO OrderHistory (OrderID, OrderDate, TotalAmount, StatusOrder, OrderType, ProductID, Quantity, Price)
    SELECT 
        o.OrderPID, 
        o.OrderDate, 
        o.TotalAmount, 
        o.StatusOrder, 
        'Purchase', -- Especificar el tipo de orden
        od.ProductID, 
        od.Quantity, 
        od.Price
    FROM OrderPurchase o
    JOIN OrderPurchaseDetail od ON o.OrderPID = od.OrderPID
    JOIN inserted i ON o.OrderPID = i.OrderPID;
END;



--A USAR
CREATE TRIGGER UpdateOrderHistoryOnSale
ON OrderSale
AFTER UPDATE
AS
BEGIN
    INSERT INTO OrderHistory (OrderID, OrderDate, TotalAmount, StatusOrder, OrderType, ProductID, Quantity, Price)
    SELECT 
        o.OrderSID, 
        o.OrderDate, 
        o.TotalAmount, 
        o.StatusOrder, 
        'Sale', -- Especificar el tipo de orden
        od.ProductID, 
        od.Quantity, 
        od.Price
    FROM OrderSale o
    JOIN OrderSaleDetail od ON o.OrderSID = od.OrderSID
    JOIN inserted i ON o.OrderSID = i.OrderSID;
END;