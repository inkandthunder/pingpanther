# pingpanther

## Getting Started

> __Setup credentials and base url__

**Add the following block to your .config file**:

```xml
<appSettings>
  <add key="hosts" value="{enter your list of hosts/machines}"/>
  <add key="to" value="{enter list of email address separated by ;}" />
  <add key="cc" value="{enter list of cc's separated by ;}" />
  <add key="from" value="{enter email of sender}" />
  <add key="subject" value="[ERROR] Host machine is down: " />
  <add key="smtpHost" value="{enter email smtp}" />
  <add key="port" value="{enter smtp port}" />
  <add key="body" value="The following host was detected as being down. Please investigate:" />
</appSettings>
```
Host keys can be limited by `space`, `,`, `.`, `:`, `;`, or `\t` 

> __Examples__

__TODO__

```C#	
TODO
```

## License

The gem is available as open source under the terms of the [MIT License](https://opensource.org/licenses/MIT).
