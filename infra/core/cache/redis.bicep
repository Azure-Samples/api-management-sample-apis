@description('The location to deploy our resources to.  Default is location of resource group')
param location string = resourceGroup().location

@description('The name of the Azure Cache for Redis instance to deploy')
param name string

@description('The tags to apply to the created resources')
param tags object = {}

@description('The pricing tier of the new Azure Cache for Redis instance')
@allowed([ 'Basic', 'Standard', 'Premium' ])
param sku string = 'Basic'

@description('Specify the size of the new Azure Redis Cache instance. Valid values: for C (Basic/Standard) family (0, 1, 2, 3, 4, 5, 6), for P (Premium) family (1, 2, 3, 4)')
@minValue(0)
@maxValue(6)
param capacity int = 1

var skuFamily = (sku == 'Premium') ? 'P' : 'C'

resource redisCache 'Microsoft.Cache/redis@2022-06-01' = {
  name: name
  location: location
  tags: tags
  properties: {
    enableNonSslPort: false
    minimumTlsVersion: '1.2'
    sku: {
      capacity: capacity
      family: skuFamily
      name: sku
    }
  }
}

output cacheName string = redisCache.name
output hostName string = redisCache.properties.hostName
