#!/usr/bin/python

import bluetooth
import time

print "Start appending devices"

file  = open("bluetoothdevices.txt", "a")

nearby_devices = bluetooth.discover_devices()
for bdaddr in nearby_devices:
    file.write(bdaddr+"\n")
    print "Found " + bdaddr

file.close()
print "End appending devices"
