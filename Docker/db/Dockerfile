FROM percona:8.0

USER root
RUN mv /etc/localtime /etc/localtime.default
RUN ln -s /usr/share/zoneinfo/America/Detroit /etc/localtime

USER mysql

COPY ./migrations /docker-entrypoint-initdb.d

COPY mysql.cnf /etc/mysql/mysql.cnf