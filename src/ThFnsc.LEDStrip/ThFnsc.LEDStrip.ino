#include <FastLED.h>
#include "SerialProtocol.h"

#define LED_PIN 13
#define PSU_PIN 14
#define NUM_LEDS 300
#define LED_TYPE WS2812B
#define COLOR_ORDER GRB

CRGB leds[NUM_LEDS];

#define UPDATES_PER_SECOND 100
#define MODE_SWITCH_INTERVAL 60000

byte globalBrightness = 255;

CRGBPalette16 currentPalette;
TBlendType currentBlending;
CRGB currentColor;
unsigned long colorReceived = 0;
unsigned long lastActivity = 0;
int lastHeap = 2147483647;
byte mode = 0;
EscapedBinaryProtocol serial(127);

void setup()
{
    Serial.begin(115200);
    Serial.println("Initializing LED Strip");
    FastLED.addLeds<LED_TYPE, LED_PIN, COLOR_ORDER>(leds, NUM_LEDS).setCorrection(TypicalLEDStrip);
    FastLED.setBrightness(255);
    Serial.println("Starting PSU");
    pinMode(PSU_PIN, OUTPUT);
    delay(1000); // power-up safety delay
    Serial.println("Boot sequence complete.");
}

bool anyLEDsOn()
{
    for (int i = 0; i < NUM_LEDS; i++)
        if (leds[i])
            return true;
    return false;
}

#define SECTIONS sizeof(sections) / sizeof(sections[0])

struct StripSection
{
    int start;
    int end;
} sections[] = {
    {0, 96},
    {96, 192}};

byte rgbBuffer[4];

byte ack[] = {1, 2, 3, 4};

void loop()
{
    int read = serial.Read();
    if (read >= 0)
    {
        switch (read)
        {
        case 3:
            currentColor = CRGB(serial.buffer[0], serial.buffer[1], serial.buffer[2]);
            colorReceived = millis();
            mode = 1;
            break;
        case 4:
            serial.Write(ack, sizeof(ack));
            break;
        }
    }
    else if (read != -1)
        Serial.printf("Error: %i\n", read);

    if (millis() % 100)
    {
        if (millis() > 43200000)
            ESP.restart();
        if (anyLEDsOn())
            lastActivity = millis();
    }

    digitalWrite(PSU_PIN, millis() - lastActivity > 10000);

    if (millis() - colorReceived > 5000)
        if (mode == 1)
            currentColor = CRGB::Black;
        else if (mode == 2)
            fill_solid(&leds[0], NUM_LEDS, CRGB::Black);

    if (mode == 1)
    {
        for (int s = 0; s < SECTIONS; s++)
        {
            int half = ((sections[s].end - sections[s].start) / 2) + sections[s].start;
            for (int i = sections[s].end - 1; i > half; i--)
                leds[i] = leds[i - 1];
            for (int i = sections[s].start; i < half; i++)
                leds[i] = leds[i + 1];
            leds[half] = currentColor;
        }
        FastLED.show();
    }
}
