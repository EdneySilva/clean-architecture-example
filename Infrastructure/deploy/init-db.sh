#./opt/mssql/bin/sqlservr & sleep 20 & /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 1234Test! -d master -i /tmp/dbinit.sql

set -e

sleep 15s & /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 1234Test! -d master -i /tmp/dbinit.sql & ./opt/mssql/bin/sqlservr