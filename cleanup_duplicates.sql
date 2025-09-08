-- Find duplicate ExplosiveCalculationResults by ProjectId and SiteId
SELECT 
    ProjectId, 
    SiteId, 
    COUNT(*) as DuplicateCount,
    STRING_AGG(CAST(Id AS VARCHAR), ', ') as DuplicateIds
FROM ExplosiveCalculationResults 
GROUP BY ProjectId, SiteId 
HAVING COUNT(*) > 1;

-- Show details of duplicate records
SELECT 
    ecr.*
FROM ExplosiveCalculationResults ecr
INNER JOIN (
    SELECT ProjectId, SiteId
    FROM ExplosiveCalculationResults 
    GROUP BY ProjectId, SiteId 
    HAVING COUNT(*) > 1
) duplicates ON ecr.ProjectId = duplicates.ProjectId AND ecr.SiteId = duplicates.SiteId
ORDER BY ecr.ProjectId, ecr.SiteId, ecr.CreatedAt;

-- Delete older duplicate records, keeping only the most recent one for each ProjectId/SiteId combination
-- UNCOMMENT THE FOLLOWING LINES TO EXECUTE THE CLEANUP:
/*
WITH DuplicateRecords AS (
    SELECT 
        Id,
        ROW_NUMBER() OVER (PARTITION BY ProjectId, SiteId ORDER BY CreatedAt DESC) as rn
    FROM ExplosiveCalculationResults
)
DELETE FROM ExplosiveCalculationResults 
WHERE Id IN (
    SELECT Id 
    FROM DuplicateRecords 
    WHERE rn > 1
);
*/