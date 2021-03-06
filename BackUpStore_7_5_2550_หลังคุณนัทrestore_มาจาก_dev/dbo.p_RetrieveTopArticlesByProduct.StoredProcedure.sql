USE [CRMQA_MSCRM]
GO
/****** Object:  StoredProcedure [dbo].[p_RetrieveTopArticlesByProduct]    Script Date: 7/5/2560 19:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


 

CREATE PROC [dbo].[p_RetrieveTopArticlesByProduct]

            @ProductId UNIQUEIDENTIFIER

AS

BEGIN

            SET NOCOUNT ON

                SELECT TOP 10 KbArticleBase.KbArticleId as kbarticleid, KbArticleBase.Title as title, T.NumCases
                FROM KbArticleBase (NOLOCK) join
                        (SELECT KbArticleId as KbArticleId, count(*) as NumCases
                     FROM IncidentBase (NOLOCK) 
                         WHERE IncidentBase.ProductId = @ProductId and IncidentBase.KbArticleId IS NOT NULL
                         GROUP BY KbArticleId) T ON KbArticleBase.KbArticleId = T.KbArticleId
                ORDER BY NumCases DESC

END

GO
