param name string
param location string = resourceGroup().location
param tags object = {}

param serviceName string = 'todo-react-web'

module web '../core/host/staticwebapp.bicep' = {
  name: '${serviceName}-staticwebapp-module'
  params: {
    name: name
    location: location
    tags: tags
  }
}

output SERVICE_WEB_NAME string = web.outputs.name
output SERVICE_WEB_URI string = web.outputs.uri
