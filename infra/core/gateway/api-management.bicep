@description('The name of the API Management service')
param name string

@description('The region where the API Management service should be deployed')
param location string = resourceGroup().location

@description('The tags that should be applied to the API Management service')
param tags object = {}

@description('The named values that should be installed for this API Management service')
param namedValues array = []

@description('The email address of the owner of the service')
@minLength(1)
param publisherEmail string = 'noreply@microsoft.com'

@description('The name of the owner of the service')
@minLength(1)
param publisherName string = 'n/a'

@description('The pricing tier of this API Management service')
@allowed([ 'Consumption', 'Developer', 'Standard', 'Premium', 'BasicV2', 'StandardV2'])
param sku string = 'BasicV2'

@description('The instance size of this API Management service.')
@allowed([ 0, 1, 2 ])
param skuCount int = 0

@description('Azure Application Insights Name')
param applicationInsightsName string

@description('Azure Cache for Redis Service Name')
param redisCacheServiceName string = ''

var redisConnectionString = !empty(redisCacheServiceName) ? '${redisCache.properties.hostName},password=${redisCache.listKeys().primaryKey},ssl=True,abortConnect=False' : ''
var redisHostName = !empty(redisCacheServiceName) ? '${redisCache.properties.hostName}' : ''

resource apimService 'Microsoft.ApiManagement/service@2023-03-01-preview' = {
  name: name
  location: location
  tags: tags
  sku: {
    name: sku
    // Consumptions requires 0, Developer 1, everything else > 0
    capacity: (sku == 'Consumption') ? 0 : ((sku == 'Developer') ? 1 : (skuCount == 0) ? 1 : skuCount)
  }
  properties: {
    publisherEmail: publisherEmail
    publisherName: publisherName
  }
}

resource apimLogger 'Microsoft.ApiManagement/service/loggers@2023-03-01-preview' = if (!empty(applicationInsightsName)) {
  name: 'app-insights-logger'
  parent: apimService
  properties: {
    credentials: {
      instrumentationKey: applicationInsights.properties.InstrumentationKey
    }
    description: 'Logger to Azure Application Insights'
    isBuffered: false
    loggerType: 'applicationInsights'
    resourceId: applicationInsights.id
  }
}

resource apimCache 'Microsoft.ApiManagement/service/caches@2023-03-01-preview' = if (!empty(redisCacheServiceName)) {
  name: 'redis-cache'
  parent: apimService
  properties: {
    connectionString: redisConnectionString
    useFromLocation: 'default'
    description: redisHostName
  }
}

resource apimNamedValue 'Microsoft.ApiManagement/service/namedValues@2023-03-01-preview' = [for nv in namedValues: {
  name: nv.key
  parent: apimService
  properties: {
    displayName: nv.key
    secret: contains(nv, 'secret') ? nv.secret : false
    value: nv.value
  }
}]

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' existing = if (!empty(applicationInsightsName)) {
  name: applicationInsightsName
}

resource redisCache 'Microsoft.Cache/redis@2022-06-01' existing = if (!empty(redisCacheServiceName)) {
  name: redisCacheServiceName
}

output serviceName string = apimService.name
output loggerName string = !empty(applicationInsightsName) ? apimLogger.name : ''
output uri string = apimService.properties.gatewayUrl
