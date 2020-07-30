FROM rabbitmq:3.8.3-management

EXPOSE 5672

WORKDIR /usr/local/bin/
COPY Infrastructure/RabbitMQ/init.sh .
RUN chmod +x ./init.sh

CMD ["./init.sh"]