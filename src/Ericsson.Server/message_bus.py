import pika
from os import environ


def create_connection():
    credentials = pika.PlainCredentials(environ['rabbitmq_username'], environ['rabbitmq_password'])
    blocking_connection = pika.BlockingConnection(pika.ConnectionParameters(environ['rabbitmq_host'], int(environ['rabbitmq_port']), '/', credentials))
    return blocking_connection


def destroy_connection(blocking_connection):
    blocking_connection.close()


def create_channel(blocking_connection):
    channel = blocking_connection.channel()
    # Declare Channel with Fanout Exchange Type
    channel.exchange_declare(exchange=environ['rabbitmq_exchange'], exchange_type='fanout')
    return channel


def publish_message(body):
    connection = create_connection()
    channel = create_channel(connection)
    channel.basic_publish(exchange=environ['rabbitmq_exchange'],
                          routing_key='',
                          body=body)
    destroy_connection(connection)
