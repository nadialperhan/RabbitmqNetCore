# RabbitmqNetCore
 RabbitMq

proje anlatımı

öncelikle .net core mvc projesi oluşturuldu kullanıcı login oldu daha sonra products sekmesine gitti
ve excel oluştura bastı burada excel dosyası ile ilgili bilgiler dbye kaydediliyor
ama henüz oluşturulmuyor

sonrasında ayrı bir proje olarak oluşturduğumuz worker service projemizde 
rabbit mq ile ilgili kuyruğu dinliyoruz

rabbitmq da şu şekilde çalışıyor

siz excel oluştura basınca ilgili kayıt dbye eklendikten sonra
publisherımız yani uygulamamız rabbitmq ya rabbitmqclientservice üzerinden bağlanıyor
ve bu sırada Exchange ve queue oluşturulup bind ediliyor daha sonra 
rabbitmqpublisher classımız üzerinden ilgili Exchange e ilgili route id ile mesajımızı gönderiyor
öncesinde Exchange ve kuyruk bind edilmişti bu Exchange ilgili mesajı kuyruğa gönderiyor

mantık şu aslında Exchange ve kuyruk oluşturulup bind edildi Exchange ve kuyruğum var artık direct Exchange olduğu için routeid ile bind edildi
daha sonra mesaj gönderdim Exchange e routeid ile exchangede ilgili mesajı bind olduğu routeid ile kuyruğa gönderdi 

  daha sonrasında worker servisim yani consumerım ilgili kuyruğa baktı kuyruk ismi ile
mesajı aldı işledi işlerken de şu adımı yaptı ilgili fileid yi aldı
önceden oluşturduğum product datası vardı onu datatable halinde alıp closedxml kütüphanesi ile excel dosyası olarak

mvc web uygulamamda oluşturduğum api controllera fileid ile gönderdi daha sonra apim de ilgili exceli alıp
root da filesa kaydetti

sonrasında kullanıcının gridde disable olan indir butonu excel fiziksel olarak kaydedildiği için aktif hale geldi
burda da kullanıcının sayfasına signalr ile mesaj gönderilip socket mantığında refresh işlemi yapıldı
böylece kullanıcı indire basınca da direkt wwwrootdaki files a ilgili dosya ismiyle ulaşıp indirebiliyor



