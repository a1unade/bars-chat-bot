VERSION=$1

cat > prod/docker-compose.yml <<EOF
services:

  api_gateway:
    image: ghcr.io/a1unade/bars-chat-bot-gateway:$VERSION
    container_name: NotifyHub.Gateway
    ports:
      - "8088:8080"
    networks:
      - notify_hub_network
    depends_on:
      - webapi
      - notification_microservice
      - outbox_processor

  webapi:
    image: ghcr.io/a1unade/bars-chat-bot-webapi:$VERSION
    container_name: NotifyHub.WebApi
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__Postgres=Host=db;Port=5432;Username=postgres;Password=notify_hub;Database=notify_hub_db;
    depends_on:
      db:
        condition: service_healthy
    ports:
      - "8080:8080"
    networks:
      - notify_hub_network
    volumes:
      - data_protection:/root/.aspnet/DataProtection-Keys
    restart: always

  notification_microservice:
    image: ghcr.io/a1unade/bars-chat-bot-notification:$VERSION
    container_name: NotifyHub.NotificationService.WebApi
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__Postgres=Host=notify_db;Port=5432;Username=postgres;Password=notify_hub_notifications;Database=notify_hub_notifications_db;
    depends_on:
      notify_db:
        condition: service_healthy
    ports:
      - "8084:8080"
    networks:
      - notify_hub_network
    volumes:
      - data_protection:/root/.aspnet/DataProtection-Keys
    restart: always

  outbox_processor:
    image: ghcr.io/a1unade/bars-chat-bot-outbox:$VERSION
    container_name: NotifyHub.OutboxProcessor
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__Postgres=Host=outbox_db;Port=5432;Username=postgres;Password=notify_hub_outbox;Database=notify_hub_outbox_db;
    depends_on:
      outbox_db:
        condition: service_healthy
    ports:
      - "8086:8080"
    networks:
      - notify_hub_network
    volumes:
      - data_protection:/root/.aspnet/DataProtection-Keys
    restart: always

  outbox_db:
    image: postgres:16
    container_name: NotifyHub.OutboxProcessor.PostgresSQL
    environment:
      TZ: UTC
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: notify_hub_outbox
      POSTGRES_DB: notify_hub_outbox_db
    command: ["postgres", "-c", "timezone=UTC"]
    volumes:
      - outbox_db_data:/var/lib/postgresql/data
    ports:
      - "5434:5432"
    networks:
      - notify_hub_network
    healthcheck:
      test: ["CMD", "pg_isready", "-U", "postgres"]
      interval: 5s
      retries: 5

  notify_db:
    image: postgres:16
    container_name: NotifyHub.NotificationService.PostgresSQL
    environment:
      TZ: UTC
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: notify_hub_notifications
      POSTGRES_DB: notify_hub_notifications_db
    command: ["postgres", "-c", "timezone=UTC"]
    volumes:
      - notify_db_data:/var/lib/postgresql/data
    ports:
      - "5433:5432"
    networks:
      - notify_hub_network
    healthcheck:
      test: ["CMD", "pg_isready", "-U", "postgres"]
      interval: 5s
      retries: 5

  db:
    image: postgres:16
    container_name: NotifyHub.PostgresSQL
    environment:
      TZ: UTC
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: notify_hub
      POSTGRES_DB: notify_hub_db
    command: ["postgres", "-c", "timezone=UTC"]
    volumes:
      - db_data:/var/lib/postgresql/data
    ports:
      - "5432:5432"
    networks:
      - notify_hub_network
    healthcheck:
      test: ["CMD", "pg_isready", "-U", "postgres"]
      interval: 5s
      retries: 5

  zookeeper:
    restart: always
    image: docker.io/bitnami/zookeeper:3.8
    ports:
      - "2181:2181"
    networks:
      - notify_hub_network
    volumes:
      - "zookeeper-volume:/bitnami"
    environment:
      - ALLOW_ANONYMOUS_LOGIN=yes

  kafka:
    restart: always
    image: docker.io/bitnami/kafka:3.3
    ports:
      - "9093:9093"
    networks:
      - notify_hub_network
    volumes:
      - "kafka-volume:/bitnami"
    environment:
      - KAFKA_BROKER_ID=1
      - KAFKA_CFG_ZOOKEEPER_CONNECT=zookeeper:2181
      - ALLOW_PLAINTEXT_LISTENER=yes
      - KAFKA_CFG_LISTENER_SECURITY_PROTOCOL_MAP=CLIENT:PLAINTEXT,EXTERNAL:PLAINTEXT
      - KAFKA_CFG_LISTENERS=CLIENT://:9092,EXTERNAL://:9093
      - KAFKA_CFG_ADVERTISED_LISTENERS=CLIENT://kafka:9092,EXTERNAL://localhost:9093
      - KAFKA_CFG_INTER_BROKER_LISTENER_NAME=CLIENT
    depends_on:
      - zookeeper

volumes:
  db_data:
  kafka-volume:
  notify_db_data:
  outbox_db_data:
  data_protection:
  zookeeper-volume:

networks:
  notify_hub_network:
    driver: bridge
EOF