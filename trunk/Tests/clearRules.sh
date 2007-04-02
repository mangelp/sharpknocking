#!/bin/sh
sudo /sbin/iptables -D INPUT 1
sudo /sbin/iptables -F SharpKnocking-INPUT
sudo /sbin/iptables -X SharpKnocking-INPUT
