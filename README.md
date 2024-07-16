# NPC Personality Vector Example

This repository contains a simplified example implementation of a personality component for non-player characters in simulation games. I made it to track how likely characters are to engage in different behaviors. Also, I wanted to be able to calculate how compatible two personalities are.

The `Personality` class represents character personalities as a vector of real-valued numbers. You can access each personality trait using a corresponding enum value. Also, you can calculate the similarity of two personalities on a scale from -1 to 1. -1 means they are diametrically opposed, and 1 means they are completely compatible.

By default, all personality attributes are clamped on the interval [-100, 100]. Setting the value of `ATTRS_MAX_ABS` will adjust this interval. For instance, `Personality<PersonalityAttrs>.ATTR_MAX_ABS = 50` will change the interval to [-50, 50]. `ATTRS_MAX_ABS` may only be a positive number.

This package can be used with Unity. You will need to write your own `PersonalityController` class to wrap the personality instance and make it available within the Inspector. See the example below.

If you have any questions, create a new GitHub issue. Don't forget to star the repository if you find this useful.

## Package Dependencies

- [MathNet.Numerics](https://www.nuget.org/packages/MathNet.Numerics/) v5.0.0

## General C\# Example

```csharp
using PersonalitySample;

// Start by defining an enum stating what the attributes of a relationship are.
public enum PersonalityAttrs
{
    VALUES_MONEY,
    VALUES_FAMILY,
    VALUES_LOYALTY,
    VALUES_POWER,
    VALUES_LOVE
}

Personality<PersonalityAttrs> personalityA = new Personality<PersonalityAttrs>();
personalityA.Set( PersonalityAttrs.VALUES_MONEY, 45 );
// Output: Personality(VALUES_MONEY=45, VALUES_FAMILY=0, VALUES_LOYALTY=0, VALUES_POWER=0, VALUES_LOVE=0)

Personality<PersonalityAttrs> personalityB = new Personality<PersonalityAttrs>();
personalityB.Set( PersonalityAttrs.VALUES_MONEY, -10 );
// Output: Personality(VALUES_MONEY=-10, VALUES_FAMILY=0, VALUES_LOYALTY=0, VALUES_POWER=0, VALUES_LOVE=0)

Console.WriteLine(personalityA.Similarity(personalityB));
// Output: -0.3
// These characters are not very compatible.

personalityB.Set( PersonalityAttrs.VALUES_MONEY, 45 );
Console.WriteLine(personalityA.Similarity(personalityB));
// Output: 1
// These characters are now 100% compatible

```

## Unity Example

```csharp
using UnityEngine;
using PersonalitySample;

public class PersonalityController : MonoBehaviour
{
    public enum PersonalityAttrs
    {
        VALUES_MONEY,
        VALUES_FAMILY,
        VALUES_LOYALTY,
        VALUES_POWER,
        VALUES_LOVE
    }

    public Personality<PersonalityAttrs> personality;

    // Initial values for personality traits
    public float values_money;
    public float values_family;
    public float values_loyalty;
    public float values_power;
    public float values_love;

    void Awake()
    {
        personality = new Personality<PersonalityAttrs>();
        personality[VALUES_MONEY] = values_money;
        personality[VALUES_FAMILY] = values_family;
        personality[VALUES_LOYALTY] = values_loyalty;
        personality[VALUES_POWER] = values_power;
        personality[VALUES_LOVE] = values_love;
    }
}

```
