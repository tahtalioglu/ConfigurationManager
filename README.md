# ConfigurationManager

Yapımız 4 ana projeden oluşmaktadır. Projeler .Net Core kullanılarak yazılmıştır.

1. ConfigurationManagement.Data-> Projede storage olan Mongodb'den veri alınırken kullanılmaktadır. 
Sistem de Mongodb'de "config" isimli database ve "records" koleksiyonu açıldıktan sonra proje ayağa kalkacaktır. 

2. ConfigurationManagement.Business -> Projenin business işlemlerinin yapıldığı kısımdır. Storage'a gidilmeden verinin Redis cache'i aracılığıyla
çekilmesini sağlayacaktır. Redis cache süresi dışarıdan alındığı için süre dolduğunda ya da veri değiştiğinde ConfigurationManagement.Data katmanındaki
IConfigurationStorage interface'i vasıtasıyla veriyi alacaktır. 

ConfigurationReader class'ı IConfigurationStorage interface'i ve ICacheManager interface'i aracılığıyla çalışır. Bu interface'leri implemente eden somut classlarımız ise
ConfigurationStorageFactory ve CacheManagerFactory aracılığıyla ayağa kalkmaktadırlar. Factory class'ları implemente edilirken Redis ve Mongodb'ye ait RedisCacheManager
ve MongoConfigurationStorage classları oluşturulacaktır.

Bununla birlikte istenen veri tipinin mevcut veriye uymaması durumunda default veri döndüren bir yapı mevcuttur.

3. ConfigurationManagement-> MVC projesi olup ConfigurationReader vasıtasıyla storage üzerinde CRUD yapan projedir.
 
4.ConfigurationManagement.Business.UnitTests-> ConfigurationReader class'ı için(Business İşlemleri bu class'ta yapıldığından) Unit Testlerin yazıldığı class'tır.
Unit testler için NUnit ve NSubstitute kullanılmıştır.
 
