file = open("bluetoothdevices.txt", "r")
lines = file.readlines()
file.close()

devicelist = []
devicelist.append("Device 03" + "\n")
devicelist.append("Device 04"+ "\n")


file = open("bluetoothdevices.txt", "w")
for line in lines:
  if line not in devicelist:
    file.write(line)
file.close()
