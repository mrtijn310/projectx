#!/usr/bin/python

import bluetooth
import RPi.GPIO as GPIO
import time
GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)
GPIO.setup(18, GPIO.OUT)
          
print "Start monitoring devices"

while True:
    print "Checking " + time.strftime("%a, %d %b %Y %H:%M:%S", time.gmtime())
    file = open("bluetoothdevices.txt", "r")
    lines = file.readlines()
    file.close()

    for line in lines:
        line = line.strip()
        #result = bluetooth.lookup_name('08:37:3D:BF:E8:3C', timeout=5)
        print line
        result = bluetooth.lookup_name(line, timeout=5)
        if (result != None):
            print "Device: in"
            GPIO.output(18, GPIO.HIGH)
        else:
            print "Device: out"
            GPIO.output(18, GPIO.LOW)
        
    time.sleep(3)
          
print "End monitoring devices"
