

აპპლიკაციის კონტეინერებს დასტარტვის მერე დარეფრეშება სჭირდებათ, რადგან ბაზა ვერ ასწრებს დასტარტვას

კომპანიის დამატება: 
curl -X 'POST' \
  'http://localhost:5016/companies' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "companyName": "opaaa"
}'


კომპანიის ინფორმაციის წამოღება:
curl -X 'GET' \
  'http://localhost:5016/companies/1' \
  -H 'accept: */*'


კომპანიის დავალიდურება:
curl -X 'POST' \
  'http://localhost:5016/companies/1/validate' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "apiKey": "298aae58-4644-4c1f-9209-455c5e6c1f37",
  "apiSecret": "ynlOnGZDmMl0Zr0rDX65dwwgqwmlVhPkGcu4lC5XrPCj12tCAD9AugIumkuB7FcQW8S2Y+RORkJ5PNRF3IP4Aw=="
}'

orderის დამატება:
curl --location 'http://localhost:5017/orders' \
--header 'API-Key: 7173ac56-892b-4402-9e19-da31e05069d8' \
--header 'API-Secret: 1zpnTSZ3JwstwbV1PQloHkQT4MYmwHsFH5TeXRgI9PeWLvw3vAOkFvz/AOLnfWjXDLG1yJEykh6MPlDRdrpXPQ==' \
--header 'Company-ID: 1' \
--header 'Content-Type: application/json' \
--data '{
  "amount": 1000,
  "currency": 1
}'

order compute:
curl --location --request GET 'http://localhost:5017/orders/compute' \
--header 'API-Key: 7173ac56-892b-4402-9e19-da31e05069d8' \
--header 'API-Secret: 1zpnTSZ3JwstwbV1PQloHkQT4MYmwHsFH5TeXRgI9PeWLvw3vAOkFvz/AOLnfWjXDLG1yJEykh6MPlDRdrpXPQ==' \
--header 'Company-ID: 1' \
--header 'Content-Type: application/json' \
--data '{
  "computationId": "3fa85f64-5717-4562-b3fc-2c963f66afa8"
}'

order compute polling:
curl --location --request GET 'http://localhost:5017/api/orders/computation-status' \
--header 'API-Key: 7173ac56-892b-4402-9e19-da31e05069d8' \
--header 'API-Secret: 1zpnTSZ3JwstwbV1PQloHkQT4MYmwHsFH5TeXRgI9PeWLvw3vAOkFvz/AOLnfWjXDLG1yJEykh6MPlDRdrpXPQ==' \
--header 'Company-ID: 1' \
--header 'Content-Type: application/json' \
--data '{
  "computationId": "3fa85f64-5717-4562-b3fc-2c963f66afa8"
}'

pay:
curl --location 'http://localhost:5018/orders' \
--header 'API-Key: 7173ac56-892b-4402-9e19-da31e05069d8' \
--header 'API-Secret: 1zpnTSZ3JwstwbV1PQloHkQT4MYmwHsFH5TeXRgI9PeWLvw3vAOkFvz/AOLnfWjXDLG1yJEykh6MPlDRdrpXPQ==' \
--header 'Company-ID: 1' \
--header 'Content-Type: application/json' \
--data '{
  "orderId": 2,
  "cardNumber": "1234567891234",
  "expiryDate": "2024-06-25T06:26:56.212Z"
}'
