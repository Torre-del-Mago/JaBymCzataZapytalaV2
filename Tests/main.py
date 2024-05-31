# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.


from selenium import webdriver
from selenium.webdriver.common.by import By
import random
import time
from selenium.common.exceptions import NoSuchElementException
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.support.ui import Select


def tryLoggingIn():
    login = driver.find_element(By.CSS_SELECTOR, "a[id = 'login']")
    login.click()

    login_input = driver.find_element(By.CSS_SELECTOR, "input[id = 'login-input']")
    login_input.send_keys("TerminatorHania")
    login_button = driver.find_element(By.CSS_SELECTOR, "button[id = 'try-login']")
    login_button.click()
    checkIfLoginIncorrect()
    time.sleep(1)
    login_input.clear()
    login_input.send_keys("zbysio")
    login_button.click()
    time.sleep(1)
    checkIfLoggedInCorrectly()

def checkIfLoginIncorrect():
    time.sleep(1)
    login_incorrect = driver.find_element(By.CSS_SELECTOR, "span[id = 'non-correct-input']")
    if(login_incorrect.text == "Login niepoprawny"):
        print("Login not correct test - passed")
    else:
        print("Login not correct test - failed")

def checkIfLoggedInCorrectly():
    print(driver.current_url)
    if driver.current_url == "http://localhost:4200/":
        print("Login correct test - passed")
    else:
        print("Login correct test - failed")

def chooseSearchOptions():
    destination = Select(
        driver.find_element(By.CSS_SELECTOR, "select[id = 'destination-select']"))
    destination.select_by_visible_text("Grecja")
    start_date = driver.find_element(By.CSS_SELECTOR, "input[id = 'start-date']")
    start_date.send_keys("01052024")
    end_date = driver.find_element(By.CSS_SELECTOR, "input[id = 'end-date']")
    end_date.send_keys("31052024")
    city = Select(
        driver.find_element(By.CSS_SELECTOR, "select[id = 'city-select']"))
    city.select_by_visible_text("Warszawa")
    add_child_button = driver.find_element(By.CSS_SELECTOR, "button[id = 'add-child-button']")
    add_child_button.click()
    search()

def search():
    search_button = driver.find_element(By.CSS_SELECTOR, "button[id = 'search-button']")
    search_button.click()
    time.sleep(5)
    try:
        driver.find_element(By.CSS_SELECTOR, "span[id = 'trip-span']")
    except NoSuchElementException:
        print("Search test - failed")
    print("Search test - passed")

def chooseTrip():
    choose_button = driver.find_element(By.CSS_SELECTOR, "button[id = 'choose-button']")
    choose_button.click()
    print(driver.current_url)
    if driver.current_url == "http://localhost:4200/detail":
        print("Choose trip test - passed")
    else:
        print("Choose trip test - failed")

options = Options()
options.accept_insecure_certs = True

driver = webdriver.Chrome()
driver.get("http://localhost:4200")

time.sleep(3)

tryLoggingIn()

time.sleep(3)

chooseSearchOptions()

time.sleep(5)

chooseTrip()

time.sleep(3)
driver.close()
# See PyCharm help at https://www.jetbrains.com/help/pycharm/
