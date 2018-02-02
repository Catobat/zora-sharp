﻿using NUnit.Framework;
using System;

namespace Zyrenth.Zora.Tests
{
	[TestFixture]
	public class GameSecretTest
	{
		const string DesiredSecretString = "H~2:@ ←2♦yq GB3●( 6♥?↑6";
		const string DesiredSecretString_JP = "かね69わ 4さをれか さ7ちわも るこぴりお";

		public static readonly GameSecret DesiredSecret = new GameSecret()
		{
			Region = GameRegion.US,
			TargetGame = Game.Ages,
			GameID = 14129,
			Hero = "Link",
			Child = "Pip",
			Animal = Animal.Dimitri,
			Behavior = 4,
			IsLinkedGame = true,
			IsHeroQuest = false,
			WasGivenFreeRing = true
		};
		
		static readonly byte[] DesiredSecretBytes = new byte[] {
			4, 37, 51, 36, 63,
			61, 51, 10, 44, 39,
			3,  0, 52, 21, 48,
			55,  9, 45, 59, 55
		};

		[Test]
		public void LoadSecretFromBytes()
		{
			GameSecret secret = new GameSecret();
			secret.Load(DesiredSecretBytes, GameRegion.US);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadSecretFromString()
		{
			GameSecret secret = new GameSecret();
			secret.Load(DesiredSecretString, GameRegion.US);

			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadFromGameInfo()
		{
			GameSecret secret = new GameSecret();
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
			GameSecret s2 = new GameSecret()
			{
				Region = GameRegion.US,
				TargetGame = Game.Ages,
				GameID = 14129,
				Hero = "Link",
				Child = "Pip",
				Animal = Animal.Dimitri,
				Behavior = 4,
				IsLinkedGame = true,
				IsHeroQuest = false,
				WasGivenFreeRing = true
			};

			Assert.AreEqual(DesiredSecret, s2);
		}

		[Test]
		public void TestNotEquals()
		{
			GameSecret s2 = new GameSecret()
			{
				Region = GameRegion.US,
				TargetGame = Game.Seasons,
				GameID = 14129,
				Hero = "Link",
				Child = "Pip",
				Animal = Animal.Dimitri,
				Behavior = 4,
				IsLinkedGame = true,
				IsHeroQuest = false,
				WasGivenFreeRing = true
			};

			Assert.AreNotEqual(DesiredSecret, s2);
			Assert.AreNotEqual(DesiredSecret, null);
			Assert.AreNotEqual(DesiredSecret, "");
		}
		
		[Test]
		public void TestInvalidByteLoad()
		{
			GameSecret secret = new GameSecret();
			Assert.Throws<SecretException>(() => secret.Load((byte[])null, GameRegion.US));
			Assert.Throws<SecretException>(() => secret.Load(new byte[] { 0 }, GameRegion.US));
			Assert.Throws<InvalidChecksumException>(() => secret.Load("H~2:@ ←2♦yq GB3●( 6♥?↑b", GameRegion.US));
			Assert.Throws<ArgumentException>(() => {
				secret.Load("L~2:N @bB↑& hmRh= HHHH↑", GameRegion.US);
			});
		}
		
		[Test]
		public void TestPalValidity()
		{
			GameSecret g1 = new GameSecret() { Hero = "Link~", Animal = Animal.Ricky };
			GameSecret g2 = new GameSecret() { Child = "Pip~", Animal = Animal.Ricky };
			GameSecret g3 = new GameSecret() { Hero = "Link", Child = "Pip", Animal = Animal.None };
			GameSecret g4 = new GameSecret() { Hero = "Link", Child = "Pip", Animal = Animal.Ricky };
			
			Assert.IsFalse(g1.IsValidForPAL(), "Hero check failed");
			Assert.IsFalse(g2.IsValidForPAL(), "Child check failed");
			Assert.IsFalse(g3.IsValidForPAL(), "Animal check failed");
			Assert.IsTrue(g4.IsValidForPAL(), "Both failed");
		}
		
	}
}

