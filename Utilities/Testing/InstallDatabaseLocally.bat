REM Install empty Subtext Database. Used for testing.
REM To run this script you have to replace all templated parameter inside the sql scripts referenced

::SET VARIABLES
SET DBNAME=%1%
IF "%DBNAME%" == "" SET DBNAME=SubtextData

OSQL -E -d %DBNAME% -i ..\..\SubTextSolution\Subtext.Installation\Scripts\Installation.01.00.00.sql
OSQL -E -d %DBNAME% -i ..\..\SubTextSolution\Subtext.Installation\Scripts\StoredProcedures.sql


PAUSE