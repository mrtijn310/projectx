from classes.ObjectView import ObjectView
from classes.PushNotification import PushNotification

def as_pushnotification(dct):
    return PushNotification(dct['Script'], dct['Params'], dct['Data'])

import json
import sys
import os
import subprocess
import time
import urllib2
import RPi.GPIO as GPIO
import zmq

GPIO.setmode(GPIO.BCM)

port = "5555"
if len(sys.argv) > 1:
    port =  sys.argv[1]
    int(port)

hostip = '192.168.192.44'
deviceid = 'C6BF9565-8C51-461C-AC70-AE94E16A6A71'	

#start off downloading the script information
webserviceUrl = "http://"+hostip+"/CrounyWeb/api/device/getscripts?deviceId="+deviceid
scripts = json.loads(urllib2.urlopen(webserviceUrl).read().decode("utf-8"))
for scriptObj in scripts:
    script = objectview(scriptObj)
    with open(script.ScriptName + ".py", "w") as text_file:
        text_file.write(script.Script)

print("Connecting to server...")
context = zmq.Context()
socket = context.socket(zmq.SUB)
socket.connect ("tcp://192.168.192.44:%s" % port)
socket.setsockopt(zmq.SUBSCRIBE, deviceid)

while (1==1):
	print("Awaiting server response.")

	#  Get the reply.
	message = socket.recv()
	if deviceid != message:
	    print("Getting ", message)
	    pushnotification = json.loads(message, object_hook = as_pushnotification)
	
	    print("Calling ",pushnotification.script)
	    subprocess.call(" python "+ pushnotification.script+" "+pushnotification.params +"", shell=True)	