import message_bus
import sys
from os import environ

try:
    #When we want to override the rabbitmq data we should set a string value into environment variable for this
    environ['override_rabbitmq']
except:
    #This is not normal way to keep secret data into code
    #But because i want to be simple way to running this application I create this rabbitmq server and put this data here
    environ['rabbitmq_username'] = 'admin'
    environ['rabbitmq_password'] = '@Ericsson2021'
    environ['rabbitmq_host'] = '65.108.54.27'
    environ['rabbitmq_port'] = '5672'
    environ['rabbitmq_exchange'] = 'main'

arg = sys.argv[0]

if bool(arg and not arg.isspace()):
    arg = 'Hello from Server'

message_bus.publish_message(sys.argv[0])

print(arg)