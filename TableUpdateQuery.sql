
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Cherish Truong>
-- Description:	<Cleans up shopStock table>
-- =============================================
CREATE PROCEDURE cleanShopStock

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT owner_id, ingredient, stock into shopStockTemp
	FROM(
	select owner_id, ingredient, sum(stock) as stock
	from dbo.shopStock t1
	group by owner_id, ingredient) as t;
	DROP TABLE shopStock;
	SELECT owner_id, ingredient, stock into dbo.shopStock
	FROM t;
	-- ask Deepali about how this should be done.

END
GO
