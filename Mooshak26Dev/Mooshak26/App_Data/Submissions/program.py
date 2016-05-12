#!/usr/bin/python
import sys

a = input()
b = input()
c = 0
c = a+b
print(c)
target = open("test.txt", 'w')
target.write("You are running the script")
sys.stdout.flush()