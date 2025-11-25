#include <Arduino.h>
#include <M5_PbHub.h>  
#include <MicroOscSlip.h>
#include <FastLED.h>
 
MicroOscSlip<128> monOsc(&Serial);
 
#define CANAL_KEY_UNIT 1  
#define KEY_CHANNEL_ANGLE 0
 
unsigned long monChronoDepart ; // À DÉPLACER au début du code avec les autres variables globales
CRGB keyPixel;
 
M5_PbHub myPbHub;
 
void myOscMessageParser(MicroOscMessage & receivedOscMessage) {
  // Ici, un if et receivedOscMessage.checkOscAddress() est utilisé pour traiter les différents messages
  if (receivedOscMessage.checkOscAddress("/color")) {  // MODIFIER /pixel pour l'adresse qui sera reçue
       int premierArgument = receivedOscMessage.nextAsInt(); // Récupérer le premier argument du message en tant que int
       int deuxiemerArgument = receivedOscMessage.nextAsInt(); // SI NÉCESSAIRE, récupérer un autre int
       int troisiemerArgument = receivedOscMessage.nextAsInt(); // SI NÉCESSAIRE, récupérer un autre int
 
       // UTILISER ici les arguments récupérés
       myPbHub.setPixelColor( CANAL_KEY_UNIT , 0 , premierArgument,deuxiemerArgument,troisiemerArgument );
       FastLED.show();
       
 
   // SI NÉCESSAIRE, ajouter d'autres if pour recevoir des messages avec d'autres adresses
   } else if (receivedOscMessage.checkOscAddress("/")) {  // MODIFIER /autre une autre adresse qui sera reçue
       // ...
   }
}
 
void setup() {
  Wire.begin();
  myPbHub.begin();
  Serial.begin(115200);
 
  myPbHub.setPixelCount(CANAL_KEY_UNIT, 1);
}
 
void loop() {
  monOsc.onOscMessageReceived(myOscMessageParser);
 
  if ( millis() - monChronoDepart >= 20 ) {
    monChronoDepart = millis();
   
    int maLectureAnalogique = myPbHub.analogRead(KEY_CHANNEL_ANGLE);
    monOsc.sendInt("/angle", maLectureAnalogique);
 
    int maLectureKey = myPbHub.digitalRead(CANAL_KEY_UNIT);
    monOsc.sendInt("/but", maLectureKey);
   
  }
}
