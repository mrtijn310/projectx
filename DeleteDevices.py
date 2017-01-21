#!/usr/bin/python

import bluetooth
import time

print "Start deleting devices"

devicelist = []
nearby_devices = bluetooth.discover_devices()
for bdaddr in nearby_devices:
    devicelist.append(bdaddr+"\n")
    print "Found " + bdaddr

file = open("bluetoothdevices.txt", "r")
lines = file.readlines()
file.close()

file = open("bluetoothdevices.txt", "w")
for line in lines:
  if line not in devicelist:
    file.write(line)
file.close()

print "End deleting devices"
