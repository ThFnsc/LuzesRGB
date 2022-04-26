#include <FastLED.h>
#include "SerialProtocol.h"

#define LED_PIN 13
#define PSU_PIN 14
#define NUM_LEDS 300
#define LED_TYPE WS2812B
#define COLOR_ORDER GRB

CRGB leds[NUM_LEDS];

CRGB currentColorLeft, currentColorRight;
unsigned long colorReceived = 0;
unsigned long lastActivity = 0;

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
    float centerOffset;
} sections[] = {
    {0, 96, .57},
    {96, 192, .57}};

byte ack[] = {1, 2, 3, 4};

void loop()
{
    int read = serial.Read();
    if (read >= 0)
    {
        switch (read)
        {
        case 6:
            currentColorLeft = CRGB(serial.buffer[0], serial.buffer[1], serial.buffer[2]);
            currentColorRight = CRGB(serial.buffer[3], serial.buffer[4], serial.buffer[5]);
            colorReceived = millis();
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
        currentColorLeft = currentColorRight = CRGB::Black;

    for (int s = 0; s < SECTIONS; s++)
    {
        int halfLeft = ((sections[s].end - sections[s].start) * sections[s].centerOffset) + sections[s].start;
        int halfRight = halfLeft + 1;
        for (int i = sections[s].end - 1; i > halfRight; i--)
            leds[i] = leds[i - 1];
        for (int i = sections[s].start; i < halfLeft; i++)
            leds[i] = leds[i + 1];
        leds[halfLeft] = currentColorRight;
        leds[halfRight] = currentColorLeft;
    }
    FastLED.show();
}
