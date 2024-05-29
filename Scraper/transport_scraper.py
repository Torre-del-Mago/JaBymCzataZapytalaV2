import random
import re
import time

import pandas as pd
from requests_html import HTMLSession

print('initialize session')

items = []

print('finished!')


def request(url):
    s = HTMLSession()
    r = s.get(url)
    r.html.render()
    elems = r.html.find('div.FlightCard-sc-14vxeb8')
    s.close()
    return elems


def output(categoryName):
    df = pd.DataFrame(items)
    df.to_csv(categoryName + '.csv', index=False)
    print('Saved to CSV file.')


def extract_data_from_category(prefixurl, categoryName):
    global items
    items = []
    print('scrapping of ' + categoryName + ' started! ')
    for departure in dep_countries_and_cities_dict:
        for key in dest_countries_and_cities_dict.keys():
            for val in dest_countries_and_cities_dict[key]:
                url = prefixurl + departure + '&arrivalRegion=' + val + '-city&adults=1&youths=0&children=0&infants=0&direction=2'
                try:
                    elem = request(url)
                    if len(elem) == 0:
                        print(f'not match: {departure} {key} {val}!')
                        continue
                    elif re.search('DateTimeInvalid', elem[0].full_text) is not None:
                        print(f'not match: {departure} {key} {val}!')
                        continue
                    item = {
                        'Wylot': departure,
                        'Kraj docelowy': key,
                        'Region docelowy': val,
                    }
                    items.append(item)
                    print(f'added: {departure} {key} {val}!')
                except:
                    print(f'Bad item: {departure} {key} {val}!')
                    continue
        print('Total Items: ', len(items))
    output(categoryName)


dep_countries_and_cities_dict = ['bydgoszcz', 'gdansk', 'katowice', 'krakow', 'lublin', 'lodz', 'olsztyn-mazury',
                                 'poznan', 'rzeszow', 'szczecin', 'warszawa', 'warszawa-radom', 'wroclaw',
                                 'zielona-gora']
dest_countries_and_cities_dict = {'brazylia': ['salvador'],
                                  'cypr': ['larnaka'],
                                  'egipt': ['hurghada', 'marsa-alam', 'sharm-el-sheikh'],
                                  'grecja': ['korfu', 'kos', 'rodos', 'samos', 'zakynthos'],
                                  'hiszpania': ['barcelona', 'ibiza', 'sewilla'],
                                  'tajlandia': ['krabi'],
                                  'tunezja': ['monastir'],
                                  'turcja': ['antalya', 'bodrum']}

print('beginning of scrapping process')
extract_data_from_category('https://biletylotnicze.itaka.pl/charter-flights?departureRegion=', 'transports')
