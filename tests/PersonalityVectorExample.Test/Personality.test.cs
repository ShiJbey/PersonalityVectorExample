using PersonalityVectorExample;

namespace PersonalityVectorExample.Test;

public class Tests
{
	[Test]
	public void TestPersonalityGet()
	{
		Personality<PersonalityAttrs> personality = new Personality<PersonalityAttrs>();

		Assert.That(personality.Get(PersonalityAttrs.VALUES_MONEY), Is.EqualTo(0f));

		Assert.That(personality[PersonalityAttrs.VALUES_MONEY], Is.EqualTo(0f));
	}

	[Test]
	public void TestPersonalitySet()
	{
		Personality<PersonalityAttrs> personality = new Personality<PersonalityAttrs>();

		personality.Set(PersonalityAttrs.VALUES_FAMILY, 20f);

		Assert.That(personality.Get(PersonalityAttrs.VALUES_FAMILY), Is.EqualTo(20f));

		personality[PersonalityAttrs.VALUES_FAMILY] = 34f;

		Assert.That(personality.Get(PersonalityAttrs.VALUES_FAMILY), Is.EqualTo(34f));
	}

	[Test]
	public void TestPersonalitySetClampsValue()
	{
		Personality<PersonalityAttrs>.ATTR_MAX_ABS = 100;

		Personality<PersonalityAttrs> personality = new Personality<PersonalityAttrs>();

		personality.Set(PersonalityAttrs.VALUES_FAMILY, 247f);

		Assert.That(personality.Get(PersonalityAttrs.VALUES_FAMILY), Is.EqualTo(100f));

		personality.Set(PersonalityAttrs.VALUES_FAMILY, -500f);

		Assert.That(personality.Get(PersonalityAttrs.VALUES_FAMILY), Is.EqualTo(-100f));
	}

	[Test]
	public void TestPersonalityCompatibility()
	{
		Personality<PersonalityAttrs> a = new Personality<PersonalityAttrs>();
		Personality<PersonalityAttrs> b = new Personality<PersonalityAttrs>();

		// This is a special case when personality attributes are all zeros
		Assert.That(a.Similarity(b), Is.EqualTo(0));

		a.Set(PersonalityAttrs.VALUES_MONEY, 10);
		b.Set(PersonalityAttrs.VALUES_MONEY, -10);

		// Now they are diametrically opposed
		Assert.That(a.Similarity(b), Is.EqualTo(-1));

		// Now they are more opposed
		b.Set(PersonalityAttrs.VALUES_MONEY, -20);
		Assert.That(a.Similarity(b), Is.EqualTo(-0.8));
	}

	public enum PersonalityAttrs
	{
		VALUES_MONEY,
		VALUES_FAMILY,
		VALUES_LOYALTY,
		VALUES_POWER,
		VALUES_LOVE
	}
}
