// using System;
// using System.Collections.Generic;
// using System.ComponentModel;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net.Http;


// namespace Umbrella.Infrastructure.Api
// {
//     public static class RequestHelpers
//     {
//         public static string ToQueryString(this object obj)
//         {
//             if (obj == null)
//                 return string.Empty;

//             var builder = new StringBuilder("?");
//             var objType = obj.GetType();

//             var parameters = objType.GetProperties()
//                                     .Select(p =>
//                                     new
//                                     {
//                                         Param = (Attribute.IsDefined(p, typeof(DescriptionAttribute)) ?
//                                                 (Attribute.GetCustomAttribute(p, typeof(DescriptionAttribute)) as DescriptionAttribute).Description
//                                                 : p.Name),
//                                         Value = Uri.EscapeDataString(p.GetValue(obj).ToString())
//                                     })
//                                     .ToList();

//             return builder.Append(string.Join("&", parameters.Select(x => $"{x.Param}={x.Value}").ToArray())).ToString();
//         }

//         public static Uri GetRequestUrl(string url, string methodName, string queryString = "")
//         {
//             var requestUrl = $"{url}{methodName}{queryString}";
//             return new Uri(requestUrl);
//         }

//         public static ByteArrayContent? CreateRequestBodyContent(object requestObject, string contentType)
//         {
//             ByteArrayContent? content = null;

//             switch (contentType)
//             {
//                 case ContentTypes.ApplicationJson:
//                     content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestObject), Encoding.UTF8, contentType);
//                     break;
//                 case ContentTypes.Text:
//                     content = new StringContent(requestObject.ToTextPlain(), Encoding.UTF8, contentType);
//                     break;
//                 case ContentTypes.FormData:
//                     content = new FormUrlEncodedContent(requestObject.ToFormData());
//                     break;
//                 default:
//                     break;
//             }

//             return content;
//         }

//         public static string ToTextPlain(this object obj)
//         {
//             if (obj == null)
//                 return string.Empty;

//             var builder = new StringBuilder();
//             var objType = obj.GetType();

//             var parameters = objType.GetProperties()
//                                     .Select(p =>
//                                     new
//                                     {
//                                         Param = (Attribute.IsDefined(p, typeof(DescriptionAttribute)) ?
//                                                 (Attribute.GetCustomAttribute(p, typeof(DescriptionAttribute)) as DescriptionAttribute).Description
//                                                 : p.Name),
//                                         Value = Uri.EscapeDataString(p.GetValue(obj).ToString())
//                                     })
//                                     .ToList();

//             return builder.Append(string.Join("&", parameters.Select(x => $"{x.Param}={x.Value}").ToArray())).ToString();
//         }

//         public static List<KeyValuePair<string, string>> ToFormData(this object obj)
//         {
//             if (obj == null)
//                 return new List<KeyValuePair<string, string>>();

//             var objType = obj.GetType();

//             var param = objType.GetProperties()
//                                     .Select(p => new KeyValuePair<string, string>(
//                                                 (Attribute.IsDefined(p, typeof(DescriptionAttribute)) ?
//                                                 (Attribute.GetCustomAttribute(p, typeof(DescriptionAttribute)) as DescriptionAttribute).Description
//                                                 : p.Name),
//                                       p.GetValue(obj).ToString()))
//                                     .ToList();

//             return param;
//         }

//     }
// }