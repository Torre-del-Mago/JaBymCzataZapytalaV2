# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.



from selenium import webdriver
from selenium.webdriver.common.by import By
import random
import time
from selenium.common.exceptions import NoSuchElementException
from selenium.webdriver.chrome.options import Options

options = Options()
options.accept_insecure_certs = True

driver = webdriver.Chrome()
driver.get("http://localhost:4200")

time.sleep(3)
driver.close()
# See PyCharm help at https://www.jetbrains.com/help/pycharm/
