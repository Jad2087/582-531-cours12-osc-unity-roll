#include <Arduino.h> 

#include <MicroOscSlip.h>  

#include <FastLED.h> 
CRGB monPixelAtom;  

#include <M5_PbHub.h> 
M5_PbHub myPbHub;

MicroOscSlip<128> monOsc(&Serial);
// Le <128> spécifie la taille du tampon pour l'OSC (c'est la quantité de données qu'il peut gérer).

#include <M5_Encoder.h>
M5_Encoder myEncoder;
  
#define CANAL_KEY_UNIT 1       

void setup()
{
 
  Serial.begin(115200);
  Wire.begin();
  myPbHub.begin(); 
  myEncoder.begin();
 
  myPbHub.setPixelCount(CANAL_KEY_UNIT, 1);

  FastLED.addLeds<WS2812, 27, GRB>(&monPixelAtom, 1); 
}

unsigned long monChronoDepart ;

void loop()
{
  // Mise à jour des valeurs de l'encodeur. 
  // Doit être appelé régulièrement.
  // Doit être appelé avant de lire les valeurs.
  myEncoder.update();
  
  // Lecture de la rotation de l'encodeur
  int valeurEncodeur = myEncoder.getEncoderRotation();

  // Lecture du changement depuis la dernière lecture
  int changementEncodeur = myEncoder.getEncoderChange();

  // Lecture du bouton 
  int etatBouton = myEncoder.getButtonState();
   
  monOsc.sendInt("/changement", changementEncodeur);
  monOsc.sendInt("/saut", etatBouton);  

  // Changer la couleur des deux pixels
  // CHANGER ROUGE, VERT, BLEU pour des valeurs entre 0 et 255 (inclusivement)

  if (millis() - monChronoDepart >= 20) {
    monChronoDepart = millis();

    if (changementEncodeur > 0) {
      // LED droite
      myEncoder.setLEDColorRight(0, 255, 0); 
      myEncoder.setLEDColorLeft(0, 0, 0);
    }
   if (changementEncodeur < 0) {
      // LED gauche
      myEncoder.setLEDColorLeft(255, 0, 0); 
      myEncoder.setLEDColorRight(0, 0, 0);
    }  
  }
}

