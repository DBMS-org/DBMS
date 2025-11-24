-- Check row counts for all tables in the database
SELECT
    t.NAME AS TableName,
    p.rows AS RowCount
FROM
    sys.tables t
INNER JOIN
    sys.indexes i ON t.OBJECT_ID = i.object_id
INNER JOIN
    sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
WHERE
    t.is_ms_shipped = 0
    AND i.index_id <= 1
GROUP BY
    t.NAME, p.Rows
ORDER BY
    p.rows DESC, t.NAME;
