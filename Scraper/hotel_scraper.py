import random
import re
import time

import pandas as pd
from requests_html import HTMLSession

print('initialize session')

s = HTMLSession()
items = []

print('finished!')


def request(url):
    r = s.get(url)
    r.html.render()
    hotels_container = r.html.xpath('//*[@class="container-lg"]', first=True)
    return [h for h in hotels_container.absolute_links if '?id=' in h]


def parse_hotel(products, country, city):
    for productLink in products:
        print(productLink)
        r = s.get(productLink)
        r.html.render()
        try:
            name = r.html.find('h1.mb-0.oui-lh-34', first=True).text
        except:
            name = 'Dobry produkt'
        try:
            desc = r.html.xpath('//*[@id="description"]', first=True).full_text
        except:
            desc = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean blandit, risus et accumsan convallis, enim dolor pharetra risus, sed laoreet ex erat ut mi. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Suspendisse non enim vitae ex sollicitudin auctor. Integer vulputate nec odio ac molestie. Nunc venenatis accumsan ex, nec molestie metus. Nullam dapibus accumsan justo, eu hendrerit augue bibendum in. Suspendisse iaculis a eros.'
        try:
            img = re.findall("https:\\/\\/wr\\.content4travel\\.com\\/itk-pl\\/img\\/\\w+0\\.jpg\\?version=\\d+-\\d+",
                             r.html.html)[0] + '&source=lib&width=1200&imgTag=1'
            try:
                img_data = s.get(img).content
                path = country + '_' + city + '_' + name + '.jpg'
                path = path.replace(' ', '_').replace('/', '_').replace('+', '_').replace('?', '_')
                with open('images_hotel/' + path, 'wb') as handler:
                    handler.write(img_data)
            except:
                continue
        except:
            continue
        print(img)

        item = {
            'Nazwa_hotelu': name,
            'Opis': '\"' + desc + '\"',
            'Kraj': country,
            'Region': city,
            'Adresy URL zdjęcia': path
        }
        items.append(item)


def getSelectedAttribute(list, name):
    for item in list:
        if name in item[0]:
            return item[1]


def output(categoryName):
    df = pd.DataFrame(items)
    df.to_csv(categoryName + '.csv', index=False)
    print('Saved to CSV file.')


def extract_data_from_category(prefixurl, categoryName):
    global items
    items = []
    print('scrapping of ' + categoryName + ' started! ')
    for key in countries_and_cities_dict.keys():
        for val in countries_and_cities_dict[key]:
            url = prefixurl + val
            x = 1
            while True:
                try:
                    products = request(url + f'/?participants%5B0%5D%5Badults%5D=1&page={x}')
                    if (len(products) == 0):
                        break
                    print(f'Getting {len(products)} items from page {x} in category: ' + key + ' ' + val)
                    parse_hotel(products, key, val)
                    print('Total Items: ', len(items))
                    x = x + 1
                    if len(items) >= max_num_of_elements:
                        break
                except:
                    print('No more items!')
                    break
            if len(items) >= max_num_of_elements:
                break
        if len(items) >= max_num_of_elements:
            break
    output(categoryName)


countries_and_cities_dict = {'brazylia': ['salvador'],
                             'cypr': ['larnaka'],
                             'egipt': ['hurghada', 'marsa-alam', 'sharm-el-sheikh'],
                             'grecja': ['korfu', 'kos', 'rodos', 'samos', 'zakynthos'],
                             'hiszpania': ['barcelona', 'ibiza', 'sewilla'],
                             'tajlandia': ['krabi'],
                             'tunezja': ['monastir'],
                             'turcja': ['antalya', 'bodrum']}

max_num_of_elements = 300
print('beginning of scrapping process')
extract_data_from_category('https://www.itaka.pl/wyniki-wyszukiwania/wakacje/', 'hotels')
