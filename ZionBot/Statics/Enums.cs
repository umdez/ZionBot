﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public static class Enums
    {
        public class stdMessages
        {
            public static string YouCannotUseThisObject = "You cannot use this object.";



        }

        public enum FollowMode : byte
        {
            Stand = 0,
            Follow = 1
        }

        public enum Priority : byte
        {
            Low = 0,
            Medium = 1,
            High = 2
        }

        public enum TargetingActions : byte
        {
            Attack,
            Ignore
        }

        public enum TargetingMode
        {
            Follow,
            FollowAtDistance,
            Stand,
            None,
            Custom
        }

        public enum Action
        {
            Walk,
            Shovel,
            Rope,
            Use,
            Ladder,
            Stand,
            Script,
            UseItemOn
        }


            public enum IncomingOpCodes : byte
            {
                GameServerLoginOrPendingState       = 10,
                GameServerGMActions                 = 11,
                GameServerEnterGame                 = 15,
                GameServerUpdateNeeded              = 17,
                GameServerLoginError                = 20,
                GameServerLoginAdvice               = 21,
                GameServerLoginWait                 = 22,
                GameServerLoginSuccess              = 23,
                GameServerLoginToken                = 24,
                GameServerStoreButtonIndicators     = 25, // 1097
                GameServerPingBack                  = 29,
                GameServerPing                      = 30,
                GameServerChallenge                 = 31,
                GameServerDeath                     = 40,

                // all in game opcodes must be greater than 50
                GameServerFirstGameOpcode           = 50,

                // otclient ONLY
                GameServerExtendedOpcode            = 50,

                // NOTE: add any custom opcodes in this range
                // 51 - 99
                GameServerChangeMapAwareRange       = 51,

                // original tibia ONLY
                GameServerFullMap                   = 100,
                GameServerMapTopRow                 = 101,
                GameServerMapRightRow               = 102,
                GameServerMapBottomRow              = 103,
                GameServerMapLeftRow                = 104,
                GameServerUpdateTile                = 105,
                GameServerCreateOnMap               = 106,
                GameServerChangeOnMap               = 107,
                GameServerDeleteOnMap               = 108,
                GameServerMoveCreature              = 109,
                GameServerOpenContainer             = 110,
                GameServerCloseContainer            = 111,
                GameServerCreateContainer           = 112,
                GameServerChangeInContainer         = 113,
                GameServerDeleteInContainer         = 114,
                GameServerSetInventory              = 120,
                GameServerDeleteInventory           = 121,
                GameServerOpenNpcTrade              = 122,
                GameServerPlayerGoods               = 123,
                GameServerCloseNpcTrade             = 124,
                GameServerOwnTrade                  = 125,
                GameServerCounterTrade              = 126,
                GameServerCloseTrade                = 127,
                GameServerAmbient                   = 130,
                GameServerGraphicalEffect           = 131,
                GameServerTextEffect                = 132,
                GameServerMissleEffect              = 133,
                GameServerMarkCreature              = 134,
                GameServerTrappers                  = 135,
                GameServerCreatureHealth            = 140,
                GameServerCreatureLight             = 141,
                GameServerCreatureOutfit            = 142,
                GameServerCreatureSpeed             = 143,
                GameServerCreatureSkull             = 144,
                GameServerCreatureParty             = 145,
                GameServerCreatureUnpass            = 146,
                GameServerCreatureMarks             = 147,
                GameServerPlayerHelpers             = 148,
                GameServerCreatureType              = 149,
                GameServerEditText                  = 150,
                GameServerEditList                  = 151,
                GameServerBlessings                 = 156,
                GameServerPreset                    = 157,
                GameServerPremiumTrigger            = 158, // 1038
                GameServerPlayerDataBasic           = 159, // 950
                GameServerPlayerData                = 160,
                GameServerPlayerSkills              = 161,
                GameServerPlayerState               = 162,
                GameServerClearTarget               = 163,
                GameServerPlayerModes               = 167,
                GameServerSpellDelay                = 164, // 870
                GameServerSpellGroupDelay           = 165, // 870
                GameServerMultiUseDelay             = 166, // 870
                GameServerSetStoreDeepLink          = 168, // 1097
                GameServerTalk                      = 170,
                GameServerChannels                  = 171,
                GameServerOpenChannel               = 172,
                GameServerOpenPrivateChannel        = 173,
                GameServerRuleViolationChannel      = 174,
                GameServerRuleViolationRemove       = 175,
                GameServerRuleViolationCancel       = 176,
                GameServerRuleViolationLock         = 177,
                GameServerOpenOwnChannel            = 178,
                GameServerCloseChannel              = 179,
                GameServerTextMessage               = 180,
                GameServerCancelWalk                = 181,
                GameServerWalkWait                  = 182,
                GameServerUnjustifiedStats          = 183,
                GameServerPvpSituations             = 184,
                GameServerFloorChangeUp             = 190,
                GameServerFloorChangeDown           = 191,
                GameServerChooseOutfit              = 200,
                GameServerVipAdd                    = 210,
                GameServerVipState                  = 211,
                GameServerVipLogout                 = 212,
                GameServerTutorialHint              = 220,
                GameServerAutomapFlag               = 221,
                GameServerCoinBalance               = 223, // 1080
                GameServerStoreError                = 224, // 1080
                GameServerRequestPurchaseData       = 225, // 1080
                GameServerQuestLog                  = 240,
                GameServerQuestLine                 = 241,
                GameServerCoinBalanceUpdating       = 242, // 1080
                GameServerChannelEvent              = 243, // 910
                GameServerItemInfo                  = 244, // 910
                GameServerPlayerInventory           = 245, // 910
                GameServerMarketEnter               = 246, // 944
                GameServerMarketLeave               = 247, // 944
                GameServerMarketDetail              = 248, // 944
                GameServerMarketBrowse              = 249, // 944
                GameServerModalDialog               = 250, // 960
                GameServerStore                     = 251, // 1080
                GameServerStoreOffers               = 252, // 1080
                GameServerStoreTransactionHistory   = 253, // 1080
                GameServerStoreCompletePurchase     = 254  // 1080
            };

        public enum SendingOpCodes : byte
        {
            ClientEnterAccount = 1,
            ClientPendingGame = 10,
            ClientEnterGame = 15,
            ClientLeaveGame = 20,
            ClientPing = 29,
            ClientPingBack = 30,

            // all in game opcodes must be equal or greater than 50
            ClientFirstGameOpcode = 50,

            // otclient ONLY
            ClientExtendedOpcode = 50,
            ClientChangeMapAwareRange = 51,

            // NOTE: add any custom opcodes in this range
            // 51 - 99

            // original tibia ONLY
            ClientAutoWalk = 100,
            ClientWalkNorth = 101,
            ClientWalkEast = 102,
            ClientWalkSouth = 103,
            ClientWalkWest = 104,
            ClientStop = 105,
            ClientWalkNorthEast = 106,
            ClientWalkSouthEast = 107,
            ClientWalkSouthWest = 108,
            ClientWalkNorthWest = 109,
            ClientTurnNorth = 111,
            ClientTurnEast = 112,
            ClientTurnSouth = 113,
            ClientTurnWest = 114,
            ClientMove = 120,
            ClientInspectNpcTrade = 121,
            ClientBuyItem = 122,
            ClientSellItem = 123,
            ClientCloseNpcTrade = 124,
            ClientRequestTrade = 125,
            ClientInspectTrade = 126,
            ClientAcceptTrade = 127,
            ClientRejectTrade = 128,
            ClientUseItem = 130,
            ClientUseItemWith = 131,
            ClientUseOnCreature = 132,
            ClientRotateItem = 133,
            ClientCloseContainer = 135,
            ClientUpContainer = 136,
            ClientEditText = 137,
            ClientEditList = 138,
            ClientLook = 140,
            ClientLookCreature = 141,
            ClientTalk = 150,
            ClientRequestChannels = 151,
            ClientJoinChannel = 152,
            ClientLeaveChannel = 153,
            ClientOpenPrivateChannel = 154,
            ClientOpenRuleViolation = 155,
            ClientCloseRuleViolation = 156,
            ClientCancelRuleViolation = 157,
            ClientCloseNpcChannel = 158,
            ClientChangeFightModes = 160,
            ClientAttack = 161,
            ClientFollow = 162,
            ClientInviteToParty = 163,
            ClientJoinParty = 164,
            ClientRevokeInvitation = 165,
            ClientPassLeadership = 166,
            ClientLeaveParty = 167,
            ClientShareExperience = 168,
            ClientDisbandParty = 169,
            ClientOpenOwnChannel = 170,
            ClientInviteToOwnChannel = 171,
            ClientExcludeFromOwnChannel = 172,
            ClientCancelAttackAndFollow = 190,
            ClientUpdateTile = 201,
            ClientRefreshContainer = 202,
            ClientBrowseField = 203,
            ClientSeekInContainer = 204,
            ClientRequestOutfit = 210,
            ClientChangeOutfit = 211,
        };








    }
}
