using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace PersonalityVectorExample
{
	/// <summary>
	/// Manages all the aspects of a character's personality.
	/// </summary>
	public class Personality<T> where T : struct, Enum
	{
		/// <summary>
		/// The absolute value of the max/min values that a personality attribute can have.
		/// </summary>
		public static float ATTR_MAX_ABS = 50;

		/// <summary>
		/// The vector of personality attributes.
		/// </summary>
		protected Vector<float> m_attributes;

		/// <summary>
		/// The vector of personality attributes.
		/// </summary>
		public Vector<float> Vector => m_attributes;

		/// <summary>
		/// The personality attributes as a dictionary.
		/// </summary>
		public IDictionary<T, float> AsDict
		{
			get
			{
				var attributeDict = new Dictionary<T, float>();

				foreach (T enumValue in Enum.GetValues(typeof(T)))
				{
					attributeDict[enumValue] = Get(enumValue);
				}

				return attributeDict;
			}
		}

		/// <summary>
		/// Construct a blank personality.
		/// </summary>
		public Personality()
		{
			m_attributes = Vector<float>.Build.Dense(Enum.GetNames(typeof(T)).Length);
		}

		/// <summary>
		/// Construct a personality with default values.
		/// </summary>
		/// <param name="defaultValues"></param>
		public Personality(IEnumerable<KeyValuePair<T, float>> defaultValues)
		{
			m_attributes = Vector<float>.Build.Dense(Enum.GetNames(typeof(T)).Length);

			foreach (var entry in defaultValues)
			{
				m_attributes[(int)(object)entry.Key] = entry.Value;
			}
		}

		/// <summary>
		/// Get an attribute value.
		/// </summary>
		/// <param name="attribute"></param>
		/// <returns></returns>
		public float Get(T attribute)
		{
			return m_attributes[(int)(object)attribute];
		}

		/// <summary>
		/// Set an attribute value.
		/// </summary>
		/// <param name="attribute"></param>
		/// <param name="value"></param>
		public void Set(T attribute, float value)
		{
			m_attributes[(int)(object)attribute] =
				Math.Min(ATTR_MAX_ABS, Math.Max(value, -ATTR_MAX_ABS));
		}

		/// <summary>
		/// Get/Set an attribute value.
		/// </summary>
		/// <param name="attribute"></param>
		/// <returns></returns>
		public float this[T attribute]
		{
			get => Get(attribute);
			set => Set(attribute, value);
		}

		/// <summary>
		/// Calculate the similarity between this personality and another.
		/// <para>
		/// This similarity measure is a combination of cosine similarity and a similarity measure
		/// of the Chebyshev distances (Infinity Norms) of the two personality vectors.
		/// </para>
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public double Similarity(Personality<T> other)
		{
			// Calculate cosine similarity
			double normProduct = this.m_attributes.L2Norm() * other.m_attributes.L2Norm();

			double cosineSimilarity = 0.0;

			if (normProduct != 0)
			{
				cosineSimilarity = this.m_attributes.DotProduct(other.m_attributes) / normProduct;
			}

			double distance = m_attributes.InfinityNorm();
			double otherDistance = other.m_attributes.InfinityNorm();

			// This similarity value is on the scale [0, 1.0]
			double chebyshevSimilarity = 1.0 - Math.Abs(distance - otherDistance) / ATTR_MAX_ABS;

			// Multiplying the similarity scores gives value on scale [-1.0, 1.0]
			return cosineSimilarity * chebyshevSimilarity;
		}

		public override string ToString()
		{
			string[] attributeValues = new string[m_attributes.Count];

			T[] enumValues = (T[])Enum.GetValues(typeof(T));

			for (int i = 0; i < enumValues.Length; i++)
			{
				attributeValues[i] = $"{enumValues[i]}={Get(enumValues[i])}";
			}

			return $"Personality({String.Join(", ", attributeValues)})";
		}
	}
}
