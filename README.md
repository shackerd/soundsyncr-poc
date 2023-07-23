# soundsyncr-poc

This is a proof of concept, i used this project for understanding how pulse audio / midi devices works and for personal use. 

I plan to write a clean version of this project in rust.

Refs:

## pulse audio (dbus)

/etc/pulse/default.pa

```
.ifexists module-dbus-protocol.so
load-module module-dbus-protocol
.endif
```

``` bash
sudo apt install libportmidi-dev
```

## portmidi

https://github.com/PortMidi/pm_csharp/tree/main/pm_managed
