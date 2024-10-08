
--exec rsp_DailyProductionSummaryReport '*','*','2023/08/01','2023/08/31'
BEGIN
SET NOCOUNT ON
DECLARE
@Section varchar(20)='*',
@Model VARCHAR(20)='*',
@FromDate datetime='2023/01/01',
@ToDate datetime='2023/08/31'

CREATE TABLE #ReportData
			(	
				ModelCode	VARCHAR(20),
				ModelName	VARCHAR(50),
				SectionCode	VARCHAR(20),
				SectionName	VARCHAR(50),
				MaterialCode	VARCHAR(20),
				MaterialName	VARCHAR(50),
				ProductionQty	DECIMAL(18,2),
				ShiftCode VARCHAR(10),
				ShiftName VARCHAR(10),
				--IssueQty	DECIMAL(18,2),
				--StockQty	DECIMAL(18,2),
				ProductionEntryDate Datetime,
				--IssueDate Datetime,
				DateFrom Datetime,
				DateTo Datetime,
			)

		-- REPORT DATA INSERT
		INSERT INTO #ReportData(ModelCode, ModelName, SectionCode, SectionName, MaterialCode, MaterialName)
		SELECT DISTINCT pm.ModelCode, pm.SectionCode,sec.SectionName,model.ModelName, pd.MaterialCode, mat.MaterialName
		FROM tblProductionMaster pm
		INNER JOIN tblProductionDetail pd ON pm.ProductionEntryNo = pd.ProductionEntryNo
		INNER JOIN tblModel model ON pm.ModelCode = model.ModelCode
		INNER JOIN tblMaterial mat ON pd.MaterialCode = mat.MaterialCode
		INNER JOIN tblSectionShop sec ON pm.SectionCode = sec.SectionCode
		INNER JOIN tblRecipeDetail rd ON pd.MaterialCode = rd.MaterialCode
		WHERE ProductionEntryDate between @FromDate and @ToDate
		AND rd.CategoryCode='SFG'
		ORDER BY pm.ModelCode, pm.SectionCode

		-- UPDATE PRODUCT QTY
		UPDATE #ReportData SET ProductionQty = t.ProductionQty,ProductionEntryDate=t.ProductionEntryDate,ShiftCode=t.ShiftCode
		FROM #ReportData rd, 
		(
		SELECT pm.ModelCode, pm.SectionCode, pd.MaterialCode, SUM(pd.ProductionQty) AS ProductionQty , ProductionEntryDate,ShiftCode
		FROM tblProductionMaster pm
		INNER JOIN tblProductionDetail pd ON pm.ProductionEntryNo = pd.ProductionEntryNo
		WHERE ProductionEntryDate between @FromDate and @ToDate
		GROUP BY pm.ModelCode, pm.SectionCode, pd.MaterialCode,ProductionEntryDate,ShiftCode
		)t
		WHERE rd.ModelCode = t.ModelCode AND rd.SectionCode = t.SectionCode AND rd.MaterialCode = t.MaterialCode



		--UPDATE SHIFT NAME 
		UPDATE rpt SET rpt.ShiftName=ss.ShiftName
		FROM #ReportData rpt
		JOIN tblShiftSetup ss ON rpt.ShiftCode=ss.ShiftCode

		--FROM DATE , TO DATE
		UPDATE #ReportData SET DateFrom=@FromDate , DateTo=@ToDate


		SELECT * 
		FROM #ReportData rd
		WHERE (rd.ModelCode=@Model OR @Model ='*')
		AND (rd.SectionCode= @Section OR @Section ='*')
		ORDER BY ModelCode
		

		DROP table #ReportData

	
END