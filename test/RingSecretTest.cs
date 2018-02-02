﻿using NUnit.Framework;
using System;

namespace Zyrenth.Zora.Tests
{
	[TestFixture]
	public class RingSecretTest
	{
		const string DesiredSecretString = "L~2:N @bB↑& hmRh=";

		public static readonly RingSecret DesiredSecret = new RingSecret()
		{
			Region = GameRegion.US,
			GameID = 14129,
			Rings = Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing
		};

		static readonly byte[] DesiredSecretBytes = new byte[] {
			6, 37, 51, 36, 13,
			63, 26,  0, 59, 47,
			30, 32, 15, 30, 49
		};

		[Test]
		public void LoadSecretFromBytes()
		{
			RingSecret secret = new RingSecret();
			secret.Load(DesiredSecretBytes, GameRegion.US);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadSecretFromString()
		{
			RingSecret secret = new RingSecret();
			secret.Load(DesiredSecretString, GameRegion.US);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadFromGameInfo()
		{
			RingSecret secret = new RingSecret();
			secret.Load(GameInfoTest.DesiredInfo);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void TestToString()
		{
			string secret = DesiredSecret.ToString();
			Assert.AreEqual(DesiredSecretString, secret);
		}

		[Test]
		public void TestToBytes()
		{
			byte[] bytes = DesiredSecret.ToBytes();	
			Assert.AreEqual(DesiredSecretBytes, bytes);
		}

		[Test]
		public void TestEquals()
		{
			RingSecret s2 = new RingSecret()
			{
				Region = GameRegion.US,
				GameID = 14129,
				Rings = Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing
			};

			Assert.AreEqual(DesiredSecret, s2);
		}

		[Test]
		public void TestNotEquals()
		{
			RingSecret s2 = new RingSecret()
			{
				Region = GameRegion.US,
				GameID = 14129,
				Rings = Rings.BlueJoyRing | Rings.BombproofRing | Rings.HundredthRing
			};

			Assert.AreNotEqual(DesiredSecret, s2);
			Assert.AreNotEqual(DesiredSecret, null);
			Assert.AreNotEqual(DesiredSecret, "");
		}
		
		[Test]
		public void TestInvalidByteLoad()
		{
			RingSecret secret = new RingSecret();
			Assert.Throws<SecretException>(() => secret.Load((byte[])null, GameRegion.US));
			Assert.Throws<SecretException>(() => secret.Load(new byte[] { 0 }, GameRegion.US));
			Assert.Throws<InvalidChecksumException>(() => secret.Load("L~2:N @bB↑& hmRhh", GameRegion.US));
			Assert.Throws<ArgumentException>(() => {
				secret.Load("H~2:@ ←2♦yq GB3●9", GameRegion.US);
			});
		}

		[Test]
		public void UpdateGameInfo()
		{
			GameInfo info = new GameInfo()
			{
				Region = GameRegion.US,
				GameID = 14129,
				Rings = Rings.PowerRingL1
			};

			// Mismatched region
			RingSecret s1 = new RingSecret()
			{
				Region = GameRegion.JP,
				GameID = 14129,
				Rings = Rings.DoubleEdgeRing | Rings.ProtectionRing
			};

			// Mismatched game ID
			RingSecret s2 = new RingSecret()
			{
				Region = GameRegion.US,
				GameID = 1,
				Rings = Rings.DoubleEdgeRing | Rings.ProtectionRing
			};

			RingSecret s3 = new RingSecret()
			{
				Region = GameRegion.US,
				GameID = 14129,
				Rings = Rings.DoubleEdgeRing | Rings.ProtectionRing
			};
			
			GameSecretTest.DesiredSecret.UpdateGameInfo(info);
			
			Assert.Throws<SecretException>(() => s1.UpdateGameInfo(info, true));
			Assert.Throws<SecretException>(() => s2.UpdateGameInfo(info, true));
			Assert.DoesNotThrow(() => s3.UpdateGameInfo(info, true));
			Assert.AreEqual(GameInfoTest.DesiredInfo, info);
		}
	}
}

