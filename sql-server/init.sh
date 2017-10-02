#!/bin/bash

/opt/mssql/bin/sqlservr.sh &

export STATUS=0
i=0
while [[ $STATUS -eq 0 ]] || [[ $i -lt 30 ]]; do
	sleep 1
	i=$i+1
	STATUS=$(grep 'Server setup is completed' /var/opt/mssql/log/setup*.log | wc -l)
done

/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -t 30 -i"/tmp/init.sql"
