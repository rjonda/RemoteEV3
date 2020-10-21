  ___               _       _____   ______     _              _ _ _                      
 | _ \___ _ __  ___| |_ ___| __\ \ / /__ /  __| |__ _ ______ | (_) |__ _ _ __ _ _ _ _  _ 
 |   / -_) '  \/ _ \  _/ -_) _| \ V / |_ \ / _| / _` (_-<_-< | | | '_ \ '_/ _` | '_| || |
 |_|_\___|_|_|_\___/\__\___|___| \_/ |___/ \__|_\__,_/__/__/ |_|_|_.__/_| \__,_|_|  \_, |
      Version 1.0.0                                                                 |__/ 
      (c) Jonáš Rosecký


 WHAT IS IT?
  It's a dll class library making PC bluetooth comunication with EV3 brick very easy.
  All you have to do is import the library and write one command to send string, number
  or boolean value.
  
 HOW IT WORKS?
  The C# based library sends and receives bluetooth packets from EV3 core brick.
  In brick just use Messaging block to send or receive data.
  
 HOW TO USE IT?
  1) Compile project with Microsoft VisualStudio(c) for newest version.
  2) Import dll library into your project.
  3) Create instances of classes EV3Receiver or EV3Sender:
        
        EV3Receiver ev3receiver = new EV3Receiver(
            "#YOUR PC INCOMMING COM PORT#",
            #FUNCTION CALLED ON DATA RECEIVED (with only parameter EV3Packet)#,
            1
        );
        
        EV3Sender ev3sender = new EV3Sender(
            "#YOUR PC OUTCOMMING COM PORT#"
        );
        
  4) Simply use:
  
        Send("#MESSAGE TITLE#", #BOOL, STRING or FLOAT MESSAGE CONTENT#);
        
  5) In LEGO MINDSTORMS(c) software just use Messaging block with same title
  6) Your pc and brick shoul now communicate ;)
  
 HOW TO GET YOUR BLUETOOTH PORT?
  1) Right click bluetooth icon in taskbar
  2) Click Properties
  3) Open COM Ports tab
  4) There should be ports for your brick
  
  If they arent there, do following things:
    a) Depending on your system connect to your brick (pair it with PC),
        reload COM Ports tab
    b) Your brick must be connected by USB wire, not by bluetooth with
        your LEGO MINDSTORMS(c) software
    c) Check, if your brick has bluetooth on and iPhone mode off, visibility on
    d) Write me at jonashrosecky@gmail.com and I'll try to solve it ;)
    
Have fun with tries ;)
I'll be glad for to feedback...