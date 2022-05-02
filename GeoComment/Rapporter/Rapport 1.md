# RESTful eller inte?

Min API är på en god väg att till att vara RESTful, men i nuläget skulle jag inte säga att det är det till 100%. Dock, som vi pratat om under lektionerna, så känns det som att det finns mycket tolkningar kring detta. 

- Uniform interface   
  > Den har en uniform interface, dock är APIn så liten att det (i inlämning 1) bara har en endpoint. På denna endpointen så används ett uniforms sätt.
- Client-Server   
  > Kravet uppfylls då klienten inte vet annat än endpoints hur de används.
- Stateless   
  > Ingen information om klienten sparas efter att anrop är gjorda. Så där uppfylls stateless kravet.
- Cacheable   
  > Jag har inte implementerat någon cachning. Därför kan vi inte säga att APIn uppfyller detta kravet.
- Layered system   
  > Det är ett layered system då databasen är/kan vara på en annan server än APIn. Det är också som så att klienten inte har någon aning om detta är fallet eller inte och behöver inte heller bry sig om det.
- Code on demenad   
  > Denna uppfylls inte, vi skicker ingen kod som kan executas tillbaka.

Kim Björnsen Åklint