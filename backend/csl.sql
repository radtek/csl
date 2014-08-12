/*
MySQL Data Transfer
Source Host: localhost
Source Database: csl
Target Host: localhost
Target Database: csl
Date: 2014/8/11 23:31:28
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for check_in
-- ----------------------------
DROP TABLE IF EXISTS `check_in`;
CREATE TABLE `check_in` (
  `id` bigint(20) NOT NULL auto_increment,
  `sys_user_id` bigint(20) NOT NULL,
  `check_in_date` date NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `sys_user_id` (`sys_user_id`),
  CONSTRAINT `check_in_ibfk_1` FOREIGN KEY (`sys_user_id`) REFERENCES `sys_user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for dict_balance_change_type
-- ----------------------------
DROP TABLE IF EXISTS `dict_balance_change_type`;
CREATE TABLE `dict_balance_change_type` (
  `id` int(11) NOT NULL default '0',
  `name` varchar(20) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for dict_chain_vote_status
-- ----------------------------
DROP TABLE IF EXISTS `dict_chain_vote_status`;
CREATE TABLE `dict_chain_vote_status` (
  `id` int(11) NOT NULL default '0',
  `name` varchar(20) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for dict_football_team
-- ----------------------------
DROP TABLE IF EXISTS `dict_football_team`;
CREATE TABLE `dict_football_team` (
  `id` int(11) NOT NULL auto_increment,
  `name` varchar(50) NOT NULL,
  `odr` int(11) default NULL,
  `delete_flag` int(11) default NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for dict_football_tournament
-- ----------------------------
DROP TABLE IF EXISTS `dict_football_tournament`;
CREATE TABLE `dict_football_tournament` (
  `id` int(11) NOT NULL,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for dict_gift_bid_status
-- ----------------------------
DROP TABLE IF EXISTS `dict_gift_bid_status`;
CREATE TABLE `dict_gift_bid_status` (
  `id` int(11) NOT NULL default '0',
  `name` varchar(20) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for dict_match_result
-- ----------------------------
DROP TABLE IF EXISTS `dict_match_result`;
CREATE TABLE `dict_match_result` (
  `id` int(11) NOT NULL,
  `name` varchar(20) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for dict_match_score_type
-- ----------------------------
DROP TABLE IF EXISTS `dict_match_score_type`;
CREATE TABLE `dict_match_score_type` (
  `id` int(11) NOT NULL,
  `name` varchar(20) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for dict_role
-- ----------------------------
DROP TABLE IF EXISTS `dict_role`;
CREATE TABLE `dict_role` (
  `id` int(11) NOT NULL default '0',
  `name` varchar(20) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for football_match
-- ----------------------------
DROP TABLE IF EXISTS `football_match`;
CREATE TABLE `football_match` (
  `id` bigint(20) NOT NULL auto_increment,
  `football_round_id` bigint(20) NOT NULL,
  `start_datetime` datetime NOT NULL,
  `home_football_team_id` int(11) NOT NULL,
  `away_football_team_id` int(11) NOT NULL,
  `vote_deadline` datetime NOT NULL,
  `end_datetime` datetime default NULL,
  `home_score` int(11) default NULL,
  `away_score` int(11) default NULL,
  `final_home_score` int(11) default NULL,
  `final_away_score` int(11) default NULL,
  `match_result_id` int(11) default NULL,
  PRIMARY KEY  (`id`),
  KEY `football_round_id` (`football_round_id`),
  KEY `home_football_team_id` (`home_football_team_id`),
  KEY `away_football_team_id` (`away_football_team_id`),
  KEY `match_result_id` (`match_result_id`),
  CONSTRAINT `football_match_ibfk_1` FOREIGN KEY (`football_round_id`) REFERENCES `football_round` (`id`),
  CONSTRAINT `football_match_ibfk_2` FOREIGN KEY (`home_football_team_id`) REFERENCES `dict_football_team` (`id`),
  CONSTRAINT `football_match_ibfk_3` FOREIGN KEY (`away_football_team_id`) REFERENCES `dict_football_team` (`id`),
  CONSTRAINT `football_match_ibfk_4` FOREIGN KEY (`match_result_id`) REFERENCES `dict_match_result` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for football_match_score
-- ----------------------------
DROP TABLE IF EXISTS `football_match_score`;
CREATE TABLE `football_match_score` (
  `id` bigint(20) NOT NULL auto_increment,
  `football_match_id` bigint(20) NOT NULL,
  `sys_user_id` bigint(20) NOT NULL,
  `match_score_type_id` int(11) NOT NULL,
  `amount` int(11) NOT NULL,
  `vote_datetime` datetime NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `football_match_id` (`football_match_id`),
  KEY `sys_user_id` (`sys_user_id`),
  KEY `match_score_type_id` (`match_score_type_id`),
  CONSTRAINT `football_match_score_ibfk_1` FOREIGN KEY (`football_match_id`) REFERENCES `football_match` (`id`),
  CONSTRAINT `football_match_score_ibfk_2` FOREIGN KEY (`sys_user_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `football_match_score_ibfk_3` FOREIGN KEY (`match_score_type_id`) REFERENCES `dict_match_score_type` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for football_match_vote
-- ----------------------------
DROP TABLE IF EXISTS `football_match_vote`;
CREATE TABLE `football_match_vote` (
  `id` bigint(20) NOT NULL auto_increment,
  `football_match_id` bigint(20) NOT NULL,
  `sys_user_id` bigint(20) NOT NULL,
  `match_result_id` int(11) NOT NULL,
  `amount` int(11) NOT NULL,
  `vote_datetime` datetime NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `football_match_id` (`football_match_id`),
  KEY `sys_user_id` (`sys_user_id`),
  KEY `match_result_id` (`match_result_id`),
  CONSTRAINT `football_match_vote_ibfk_1` FOREIGN KEY (`football_match_id`) REFERENCES `football_match` (`id`),
  CONSTRAINT `football_match_vote_ibfk_2` FOREIGN KEY (`sys_user_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `football_match_vote_ibfk_3` FOREIGN KEY (`match_result_id`) REFERENCES `dict_match_result` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for football_round
-- ----------------------------
DROP TABLE IF EXISTS `football_round`;
CREATE TABLE `football_round` (
  `id` bigint(20) NOT NULL auto_increment,
  `football_tournament_id` int(11) NOT NULL,
  `year` int(11) NOT NULL,
  `name` varchar(20) NOT NULL,
  `start_datetime` datetime NOT NULL,
  `end_datetime` datetime NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `football_tournament_id` (`football_tournament_id`),
  CONSTRAINT `football_round_ibfk_1` FOREIGN KEY (`football_tournament_id`) REFERENCES `dict_football_tournament` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for football_round_chain
-- ----------------------------
DROP TABLE IF EXISTS `football_round_chain`;
CREATE TABLE `football_round_chain` (
  `id` bigint(20) NOT NULL auto_increment,
  `football_round_id` bigint(20) NOT NULL,
  `name` varchar(50) NOT NULL,
  `participant_amount` int(11) NOT NULL,
  `gift_id` bigint(20) default NULL,
  `price` int(11) default NULL,
  `vote_deadline` datetime NOT NULL,
  `odr` int(11) default NULL,
  `delete_flag` int(11) default NULL,
  PRIMARY KEY  (`id`),
  KEY `football_round_id` (`football_round_id`),
  KEY `gift_id` (`gift_id`),
  CONSTRAINT `football_round_chain_ibfk_1` FOREIGN KEY (`football_round_id`) REFERENCES `football_round` (`id`),
  CONSTRAINT `football_round_chain_ibfk_2` FOREIGN KEY (`gift_id`) REFERENCES `gift` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for football_round_chain_vote
-- ----------------------------
DROP TABLE IF EXISTS `football_round_chain_vote`;
CREATE TABLE `football_round_chain_vote` (
  `id` bigint(20) NOT NULL auto_increment,
  `football_round_chain_id` bigint(20) NOT NULL,
  `sys_user_id` bigint(20) NOT NULL,
  `participant_need` int(11) NOT NULL,
  `chain_vote_status_id` int(11) NOT NULL,
  `vote_datetime` datetime NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `football_round_chain_id` (`football_round_chain_id`),
  KEY `sys_user_id` (`sys_user_id`),
  KEY `chain_vote_status_id` (`chain_vote_status_id`),
  CONSTRAINT `football_round_chain_vote_ibfk_1` FOREIGN KEY (`football_round_chain_id`) REFERENCES `football_round_chain` (`id`),
  CONSTRAINT `football_round_chain_vote_ibfk_2` FOREIGN KEY (`sys_user_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `football_round_chain_vote_ibfk_3` FOREIGN KEY (`chain_vote_status_id`) REFERENCES `dict_chain_vote_status` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for football_round_chain_vote_invite
-- ----------------------------
DROP TABLE IF EXISTS `football_round_chain_vote_invite`;
CREATE TABLE `football_round_chain_vote_invite` (
  `id` bigint(20) NOT NULL auto_increment,
  `football_round_chain_vote_id` bigint(20) NOT NULL,
  `sys_user_id` bigint(20) default NULL,
  `football_match_id` bigint(20) NOT NULL,
  `match_result_id` int(11) default NULL,
  `vote_datetime` datetime default NULL,
  PRIMARY KEY  (`id`),
  KEY `football_round_chain_vote_id` (`football_round_chain_vote_id`),
  KEY `sys_user_id` (`sys_user_id`),
  KEY `football_match_id` (`football_match_id`),
  KEY `match_result_id` (`match_result_id`),
  CONSTRAINT `football_round_chain_vote_invite_ibfk_1` FOREIGN KEY (`football_round_chain_vote_id`) REFERENCES `football_round_chain_vote` (`id`),
  CONSTRAINT `football_round_chain_vote_invite_ibfk_2` FOREIGN KEY (`sys_user_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `football_round_chain_vote_invite_ibfk_3` FOREIGN KEY (`football_match_id`) REFERENCES `football_match` (`id`),
  CONSTRAINT `football_round_chain_vote_invite_ibfk_4` FOREIGN KEY (`match_result_id`) REFERENCES `dict_match_result` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for gift
-- ----------------------------
DROP TABLE IF EXISTS `gift`;
CREATE TABLE `gift` (
  `id` bigint(20) NOT NULL auto_increment,
  `title` varchar(50) NOT NULL,
  `description` varchar(1000) default NULL,
  `price` int(11) NOT NULL default '0',
  `inventory` int(11) default NULL,
  `off_shelf_datetime` datetime default NULL,
  `odr` bigint(20) default NULL,
  `delete_flag` int(11) default NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for gift_auction
-- ----------------------------
DROP TABLE IF EXISTS `gift_auction`;
CREATE TABLE `gift_auction` (
  `id` bigint(20) NOT NULL auto_increment,
  `gift_id` bigint(20) NOT NULL,
  `title` varchar(50) NOT NULL,
  `description` varchar(1000) NOT NULL,
  `min_bid_amount` int(11) NOT NULL,
  `start_datetime` datetime NOT NULL,
  `end_datetime` datetime NOT NULL,
  `delete_flag` int(11) default NULL,
  PRIMARY KEY  (`id`),
  KEY `gift_id` (`gift_id`),
  CONSTRAINT `gift_auction_ibfk_1` FOREIGN KEY (`gift_id`) REFERENCES `gift` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for gift_auction_sys_user_bid
-- ----------------------------
DROP TABLE IF EXISTS `gift_auction_sys_user_bid`;
CREATE TABLE `gift_auction_sys_user_bid` (
  `id` bigint(20) NOT NULL auto_increment,
  `gift_auction_id` bigint(20) NOT NULL,
  `sys_user_id` bigint(20) NOT NULL,
  `amount` int(11) NOT NULL,
  `gift_bid_status_id` int(11) NOT NULL,
  `bid_datetime` datetime NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `gift_auction_id` (`gift_auction_id`),
  KEY `sys_user_id` (`sys_user_id`),
  KEY `gift_bid_status_id` (`gift_bid_status_id`),
  CONSTRAINT `gift_auction_sys_user_bid_ibfk_1` FOREIGN KEY (`gift_auction_id`) REFERENCES `gift_auction` (`id`),
  CONSTRAINT `gift_auction_sys_user_bid_ibfk_2` FOREIGN KEY (`sys_user_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `gift_auction_sys_user_bid_ibfk_3` FOREIGN KEY (`gift_bid_status_id`) REFERENCES `dict_gift_bid_status` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for gift_history
-- ----------------------------
DROP TABLE IF EXISTS `gift_history`;
CREATE TABLE `gift_history` (
  `id` bigint(20) NOT NULL auto_increment,
  `gift_id` bigint(20) NOT NULL,
  `sys_user_id` bigint(20) NOT NULL,
  `buy_datetime` datetime NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `gift_id` (`gift_id`),
  KEY `sys_user_id` (`sys_user_id`),
  CONSTRAINT `gift_history_ibfk_1` FOREIGN KEY (`gift_id`) REFERENCES `gift` (`id`),
  CONSTRAINT `gift_history_ibfk_2` FOREIGN KEY (`sys_user_id`) REFERENCES `sys_user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for sys_message
-- ----------------------------
DROP TABLE IF EXISTS `sys_message`;
CREATE TABLE `sys_message` (
  `id` bigint(20) NOT NULL auto_increment,
  `title` varchar(50) NOT NULL,
  `detail` varchar(1000) NOT NULL,
  `receive_sys_user_id` bigint(20) default NULL,
  `send_sys_user_id` bigint(20) NOT NULL,
  `send_datetime` datetime NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `receive_sys_user_id` (`receive_sys_user_id`),
  KEY `send_sys_user_id` (`send_sys_user_id`),
  CONSTRAINT `sys_message_ibfk_1` FOREIGN KEY (`receive_sys_user_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `sys_message_ibfk_2` FOREIGN KEY (`send_sys_user_id`) REFERENCES `sys_user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for sys_question
-- ----------------------------
DROP TABLE IF EXISTS `sys_question`;
CREATE TABLE `sys_question` (
  `id` bigint(20) NOT NULL auto_increment,
  `title` varchar(50) NOT NULL,
  `detail` varchar(1000) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for sys_user
-- ----------------------------
DROP TABLE IF EXISTS `sys_user`;
CREATE TABLE `sys_user` (
  `id` bigint(20) NOT NULL auto_increment,
  `login_name` varchar(50) NOT NULL,
  `login_pwd` varchar(50) NOT NULL,
  `login_pwd_guid` varchar(36) NOT NULL,
  `role_id` int(11) NOT NULL,
  `name` varchar(50) default NULL,
  `phone` varchar(50) default NULL,
  `address` varchar(200) default NULL,
  `email` varchar(50) default NULL,
  `qq` varchar(50) default NULL,
  `weixin` varchar(50) default NULL COMMENT '微信',
  `balance` decimal(10,2) NOT NULL default '0.00',
  `reference_sys_user_id` bigint(20) default NULL,
  `register_date` datetime NOT NULL,
  `last_login_date` datetime default NULL,
  `last_login_ip` varchar(50) default NULL,
  `delete_flag` int(11) default NULL,
  PRIMARY KEY  (`id`),
  KEY `role_id` (`role_id`),
  KEY `reference_sys_user_id` (`reference_sys_user_id`),
  CONSTRAINT `sys_user_ibfk_1` FOREIGN KEY (`role_id`) REFERENCES `dict_role` (`id`),
  CONSTRAINT `sys_user_ibfk_2` FOREIGN KEY (`reference_sys_user_id`) REFERENCES `sys_user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for sys_user_balance_change
-- ----------------------------
DROP TABLE IF EXISTS `sys_user_balance_change`;
CREATE TABLE `sys_user_balance_change` (
  `id` bigint(20) NOT NULL auto_increment,
  `sys_user_id` bigint(20) NOT NULL,
  `balance_change_type_id` int(11) NOT NULL,
  `amount` decimal(10,2) NOT NULL,
  `balance_change_datetime` datetime NOT NULL,
  `remark` varchar(100) default NULL,
  `ref_table` varchar(50) default NULL,
  `ref_id` varchar(100) default NULL,
  PRIMARY KEY  (`id`),
  KEY `sys_user_id` (`sys_user_id`),
  KEY `balance_change_type_id` (`balance_change_type_id`),
  CONSTRAINT `sys_user_balance_change_ibfk_1` FOREIGN KEY (`sys_user_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `sys_user_balance_change_ibfk_2` FOREIGN KEY (`balance_change_type_id`) REFERENCES `dict_balance_change_type` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8;

-- ----------------------------
-- View structure for check_in_view
-- ----------------------------
DROP VIEW IF EXISTS `check_in_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `check_in_view` AS select `check_in`.`id` AS `id`,`check_in`.`sys_user_id` AS `sys_user_id`,`check_in`.`check_in_date` AS `check_in_date`,`sys_user`.`login_name` AS `sys_user_login_name` from (`check_in` left join `sys_user` on((`sys_user`.`id` = `check_in`.`sys_user_id`)));

-- ----------------------------
-- View structure for football_match_score_view
-- ----------------------------
DROP VIEW IF EXISTS `football_match_score_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `football_match_score_view` AS select `football_match`.`football_round_id` AS `football_round_id`,`football_match`.`start_datetime` AS `start_datetime`,`football_match`.`home_football_team_id` AS `home_football_team_id`,`football_match`.`away_football_team_id` AS `away_football_team_id`,`football_match`.`vote_deadline` AS `vote_deadline`,`football_match`.`end_datetime` AS `end_datetime`,`football_match`.`home_score` AS `home_score`,`football_match`.`away_score` AS `away_score`,`football_match`.`final_home_score` AS `final_home_score`,`football_match`.`final_away_score` AS `final_away_score`,`football_match`.`match_result_id` AS `match_result_id`,`football_round`.`year` AS `football_round_year`,`football_round`.`name` AS `football_round_name`,`home_team`.`name` AS `home_football_team_name`,`away_team`.`name` AS `away_football_team_name`,`dict_match_result`.`name` AS `match_result_name`,`football_match_score`.`id` AS `id`,`football_match_score`.`football_match_id` AS `football_match_id`,`football_match_score`.`sys_user_id` AS `sys_user_id`,`football_match_score`.`match_score_type_id` AS `match_score_type_id`,`football_match_score`.`amount` AS `amount`,`football_match_score`.`vote_datetime` AS `vote_datetime`,`sys_user`.`login_name` AS `sys_user_login_name`,`dict_match_score_type`.`name` AS `match_score_type_name` from (((`football_match_score` left join ((((`football_match` left join `football_round` on((`football_round`.`id` = `football_match`.`football_round_id`))) left join `dict_football_team` `home_team` on((`home_team`.`id` = `football_match`.`home_football_team_id`))) left join `dict_football_team` `away_team` on((`away_team`.`id` = `football_match`.`away_football_team_id`))) left join `dict_match_result` on((`dict_match_result`.`id` = `football_match`.`match_result_id`))) on((`football_match_score`.`football_match_id` = `football_match`.`id`))) left join `sys_user` on((`sys_user`.`id` = `football_match_score`.`sys_user_id`))) left join `dict_match_score_type` on((`dict_match_score_type`.`id` = `football_match_score`.`match_score_type_id`)));

-- ----------------------------
-- View structure for football_match_view
-- ----------------------------
DROP VIEW IF EXISTS `football_match_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `football_match_view` AS select `football_match`.`id` AS `id`,`football_match`.`football_round_id` AS `football_round_id`,`football_match`.`start_datetime` AS `start_datetime`,`football_match`.`home_football_team_id` AS `home_football_team_id`,`football_match`.`away_football_team_id` AS `away_football_team_id`,`football_match`.`vote_deadline` AS `vote_deadline`,`football_match`.`end_datetime` AS `end_datetime`,`football_match`.`home_score` AS `home_score`,`football_match`.`away_score` AS `away_score`,`football_match`.`final_home_score` AS `final_home_score`,`football_match`.`final_away_score` AS `final_away_score`,`football_match`.`match_result_id` AS `match_result_id`,`football_round`.`year` AS `football_round_year`,`football_round`.`name` AS `football_round_name`,`home_team`.`name` AS `home_football_team_name`,`away_team`.`name` AS `away_football_team_name`,`dict_match_result`.`name` AS `match_result_name` from ((((`football_match` left join `football_round` on((`football_round`.`id` = `football_match`.`football_round_id`))) left join `dict_football_team` `home_team` on((`home_team`.`id` = `football_match`.`home_football_team_id`))) left join `dict_football_team` `away_team` on((`away_team`.`id` = `football_match`.`away_football_team_id`))) left join `dict_match_result` on((`dict_match_result`.`id` = `football_match`.`match_result_id`)));

-- ----------------------------
-- View structure for football_match_vote_view
-- ----------------------------
DROP VIEW IF EXISTS `football_match_vote_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `football_match_vote_view` AS select `football_match`.`football_round_id` AS `football_round_id`,`football_match`.`start_datetime` AS `start_datetime`,`football_match`.`home_football_team_id` AS `home_football_team_id`,`football_match`.`away_football_team_id` AS `away_football_team_id`,`football_match`.`vote_deadline` AS `vote_deadline`,`football_match`.`end_datetime` AS `end_datetime`,`football_match`.`home_score` AS `home_score`,`football_match`.`away_score` AS `away_score`,`football_match`.`final_home_score` AS `final_home_score`,`football_match`.`final_away_score` AS `final_away_score`,`football_match`.`match_result_id` AS `match_result_id`,`football_round`.`year` AS `football_round_year`,`football_round`.`name` AS `football_round_name`,`home_team`.`name` AS `home_football_team_name`,`away_team`.`name` AS `away_football_team_name`,`dict_match_result`.`name` AS `match_result_name`,`football_match_vote`.`id` AS `id`,`football_match_vote`.`football_match_id` AS `football_match_id`,`football_match_vote`.`sys_user_id` AS `sys_user_id`,`football_match_vote`.`match_result_id` AS `vote_match_result_id`,`football_match_vote`.`amount` AS `amount`,`football_match_vote`.`vote_datetime` AS `vote_datetime`,`vote_match_result`.`name` AS `vote_match_result_name`,`sys_user`.`login_name` AS `sys_user_login_name` from (((`football_match_vote` left join ((((`football_match` left join `football_round` on((`football_round`.`id` = `football_match`.`football_round_id`))) left join `dict_football_team` `home_team` on((`home_team`.`id` = `football_match`.`home_football_team_id`))) left join `dict_football_team` `away_team` on((`away_team`.`id` = `football_match`.`away_football_team_id`))) left join `dict_match_result` on((`dict_match_result`.`id` = `football_match`.`match_result_id`))) on((`football_match`.`id` = `football_match_vote`.`football_match_id`))) left join `dict_match_result` `vote_match_result` on((`vote_match_result`.`id` = `football_match_vote`.`match_result_id`))) left join `sys_user` on((`sys_user`.`id` = `football_match_vote`.`sys_user_id`)));

-- ----------------------------
-- View structure for football_round_chain_view
-- ----------------------------
DROP VIEW IF EXISTS `football_round_chain_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `football_round_chain_view` AS select `football_round_chain`.`id` AS `id`,`football_round_chain`.`football_round_id` AS `football_round_id`,`football_round_chain`.`name` AS `name`,`football_round_chain`.`participant_amount` AS `participant_amount`,`football_round_chain`.`gift_id` AS `gift_id`,`football_round_chain`.`price` AS `price`,`football_round_chain`.`vote_deadline` AS `vote_deadline`,`football_round_chain`.`odr` AS `odr`,`football_round_chain`.`delete_flag` AS `delete_flag`,`football_round`.`year` AS `year`,`football_round`.`name` AS `football_round_name`,`gift`.`title` AS `gift_title` from ((`football_round_chain` left join `football_round` on((`football_round`.`id` = `football_round_chain`.`football_round_id`))) left join `gift` on((`gift`.`id` = `football_round_chain`.`gift_id`)));

-- ----------------------------
-- View structure for football_round_chain_vote_invite_view
-- ----------------------------
DROP VIEW IF EXISTS `football_round_chain_vote_invite_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `football_round_chain_vote_invite_view` AS select `football_round_chain_vote_invite`.`id` AS `id`,`football_round_chain_vote_invite`.`football_round_chain_vote_id` AS `football_round_chain_vote_id`,`football_round_chain_vote_invite`.`sys_user_id` AS `vote_sys_user_id`,`football_round_chain_vote_invite`.`football_match_id` AS `football_match_id`,`football_round_chain_vote_invite`.`match_result_id` AS `vote_match_result_id`,`football_round_chain_vote_invite`.`vote_datetime` AS `vote_datetime`,`football_round_chain_vote`.`football_round_chain_id` AS `football_round_chain_id`,`football_round_chain_vote`.`sys_user_id` AS `sys_user_id`,`football_round_chain_vote`.`chain_vote_status_id` AS `chain_vote_status_id`,`football_round_chain`.`football_round_id` AS `football_round_id`,`football_round_chain`.`name` AS `football_round_chain_name`,`football_round_chain`.`gift_id` AS `gift_id`,`football_round_chain`.`price` AS `price`,`gift`.`title` AS `gift_title`,`football_round`.`year` AS `football_round_year`,`football_round`.`name` AS `football_round_name`,`dict_chain_vote_status`.`name` AS `chain_vote_status_name`,`vote_sys_user`.`login_name` AS `vote_sys_user_login_name`,`vote_match_result`.`name` AS `vote_match_result_name`,`sys_user`.`login_name` AS `sys_user_login_name`,`football_match`.`home_football_team_id` AS `home_football_team_id`,`football_match`.`away_football_team_id` AS `away_football_team_id`,`football_match`.`match_result_id` AS `match_result_id`,`home_team`.`name` AS `home_football_team_name`,`away_team`.`name` AS `away_football_team_name`,`dict_match_result`.`name` AS `match_result_name` from ((((((((((((`football_round_chain_vote_invite` left join `football_round_chain_vote` on((`football_round_chain_vote`.`id` = `football_round_chain_vote_invite`.`football_round_chain_vote_id`))) left join `football_round_chain` on((`football_round_chain`.`id` = `football_round_chain_vote`.`football_round_chain_id`))) left join `gift` on((`gift`.`id` = `football_round_chain`.`gift_id`))) left join `football_round` on((`football_round`.`id` = `football_round_chain`.`football_round_id`))) left join `dict_chain_vote_status` on((`dict_chain_vote_status`.`id` = `football_round_chain_vote`.`chain_vote_status_id`))) left join `sys_user` `vote_sys_user` on((`vote_sys_user`.`id` = `football_round_chain_vote_invite`.`sys_user_id`))) left join `dict_match_result` `vote_match_result` on((`vote_match_result`.`id` = `football_round_chain_vote_invite`.`match_result_id`))) left join `sys_user` on((`sys_user`.`id` = `football_round_chain_vote`.`sys_user_id`))) left join `football_match` on((`football_match`.`id` = `football_round_chain_vote_invite`.`football_match_id`))) left join `dict_football_team` `home_team` on((`home_team`.`id` = `football_match`.`home_football_team_id`))) left join `dict_football_team` `away_team` on((`away_team`.`id` = `football_match`.`away_football_team_id`))) left join `dict_match_result` on((`dict_match_result`.`id` = `football_match`.`match_result_id`)));

-- ----------------------------
-- View structure for football_round_chain_vote_view
-- ----------------------------
DROP VIEW IF EXISTS `football_round_chain_vote_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `football_round_chain_vote_view` AS select `football_round_chain_vote`.`id` AS `id`,`football_round_chain_vote`.`football_round_chain_id` AS `football_round_chain_id`,`football_round_chain_vote`.`sys_user_id` AS `sys_user_id`,`football_round_chain_vote`.`participant_need` AS `participant_need`,`football_round_chain_vote`.`chain_vote_status_id` AS `chain_vote_status_id`,`football_round_chain_vote`.`vote_datetime` AS `vote_datetime`,`football_round_chain`.`football_round_id` AS `football_round_id`,`football_round_chain`.`name` AS `football_round_chain_name`,`football_round_chain`.`gift_id` AS `gift_id`,`football_round_chain`.`price` AS `price`,`football_round`.`year` AS `football_round_year`,`football_round`.`name` AS `football_round_name`,`gift`.`title` AS `gift_title`,`sys_user`.`login_name` AS `sys_user_login_name`,`dict_chain_vote_status`.`name` AS `chain_vote_status_name` from (((((`football_round_chain_vote` left join `football_round_chain` on((`football_round_chain`.`id` = `football_round_chain_vote`.`football_round_chain_id`))) left join `football_round` on((`football_round`.`id` = `football_round_chain`.`football_round_id`))) left join `gift` on((`gift`.`id` = `football_round_chain`.`gift_id`))) left join `sys_user` on((`sys_user`.`id` = `football_round_chain_vote`.`sys_user_id`))) left join `dict_chain_vote_status` on((`dict_chain_vote_status`.`id` = `football_round_chain_vote`.`chain_vote_status_id`)));

-- ----------------------------
-- View structure for football_round_view
-- ----------------------------
DROP VIEW IF EXISTS `football_round_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `football_round_view` AS select `football_round`.`id` AS `id`,`football_round`.`football_tournament_id` AS `football_tournament_id`,`football_round`.`year` AS `year`,`football_round`.`name` AS `name`,`football_round`.`start_datetime` AS `start_datetime`,`football_round`.`end_datetime` AS `end_datetime`,`dict_football_tournament`.`name` AS `football_tournament_name` from (`football_round` left join `dict_football_tournament` on((`dict_football_tournament`.`id` = `football_round`.`football_tournament_id`)));

-- ----------------------------
-- View structure for gift_auction_sys_user_bid_view
-- ----------------------------
DROP VIEW IF EXISTS `gift_auction_sys_user_bid_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `gift_auction_sys_user_bid_view` AS select `gift_auction_sys_user_bid`.`id` AS `id`,`gift_auction_sys_user_bid`.`gift_auction_id` AS `gift_auction_id`,`gift_auction_sys_user_bid`.`sys_user_id` AS `sys_user_id`,`gift_auction_sys_user_bid`.`amount` AS `amount`,`gift_auction_sys_user_bid`.`gift_bid_status_id` AS `gift_bid_status_id`,`gift_auction_sys_user_bid`.`bid_datetime` AS `bid_datetime`,`gift_auction`.`title` AS `auction_title`,`gift_auction`.`gift_id` AS `gift_id`,`gift`.`title` AS `gift_title`,`sys_user`.`login_name` AS `sys_user_login_name`,`dict_gift_bid_status`.`name` AS `gift_bid_status_name` from ((((`gift_auction_sys_user_bid` left join `gift_auction` on((`gift_auction`.`id` = `gift_auction_sys_user_bid`.`gift_auction_id`))) left join `gift` on((`gift`.`id` = `gift_auction`.`gift_id`))) left join `sys_user` on((`sys_user`.`id` = `gift_auction_sys_user_bid`.`sys_user_id`))) left join `dict_gift_bid_status` on((`dict_gift_bid_status`.`id` = `gift_auction_sys_user_bid`.`gift_bid_status_id`)));

-- ----------------------------
-- View structure for gift_auction_view
-- ----------------------------
DROP VIEW IF EXISTS `gift_auction_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `gift_auction_view` AS select `gift_auction`.`id` AS `id`,`gift_auction`.`gift_id` AS `gift_id`,`gift_auction`.`title` AS `title`,`gift_auction`.`description` AS `description`,`gift_auction`.`min_bid_amount` AS `min_bid_amount`,`gift_auction`.`start_datetime` AS `start_datetime`,`gift_auction`.`end_datetime` AS `end_datetime`,`gift_auction`.`delete_flag` AS `delete_flag`,`gift`.`title` AS `gift_title` from (`gift_auction` left join `gift` on((`gift`.`id` = `gift_auction`.`gift_id`)));

-- ----------------------------
-- View structure for gift_history_view
-- ----------------------------
DROP VIEW IF EXISTS `gift_history_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `gift_history_view` AS select `gift_history`.`id` AS `id`,`gift_history`.`gift_id` AS `gift_id`,`gift_history`.`sys_user_id` AS `sys_user_id`,`gift_history`.`buy_datetime` AS `buy_datetime`,`gift`.`title` AS `title`,`gift`.`price` AS `price`,`sys_user`.`login_name` AS `login_name` from ((`gift_history` left join `gift` on((`gift`.`id` = `gift_history`.`gift_id`))) left join `sys_user` on((`sys_user`.`id` = `gift_history`.`sys_user_id`)));

-- ----------------------------
-- View structure for sys_message_view
-- ----------------------------
DROP VIEW IF EXISTS `sys_message_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `sys_message_view` AS select `sys_message`.`id` AS `id`,`sys_message`.`title` AS `title`,`sys_message`.`detail` AS `detail`,`sys_message`.`receive_sys_user_id` AS `receive_sys_user_id`,`sys_message`.`send_sys_user_id` AS `send_sys_user_id`,`sys_message`.`send_datetime` AS `send_datetime`,`receive_sys_user`.`login_name` AS `receive_sys_user_login_name`,`send_sys_user`.`login_name` AS `send_sys_user_login_name` from ((`sys_message` left join `sys_user` `receive_sys_user` on((`receive_sys_user`.`id` = `sys_message`.`receive_sys_user_id`))) left join `sys_user` `send_sys_user` on((`send_sys_user`.`id` = `sys_message`.`send_sys_user_id`)));

-- ----------------------------
-- View structure for sys_user_balance_change_view
-- ----------------------------
DROP VIEW IF EXISTS `sys_user_balance_change_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `sys_user_balance_change_view` AS select `sys_user_balance_change`.`id` AS `id`,`sys_user_balance_change`.`sys_user_id` AS `sys_user_id`,`sys_user_balance_change`.`balance_change_type_id` AS `balance_change_type_id`,`sys_user_balance_change`.`balance_change_datetime` AS `balance_change_datetime`,`sys_user_balance_change`.`remark` AS `remark`,`sys_user_balance_change`.`ref_table` AS `ref_table`,`sys_user_balance_change`.`ref_id` AS `ref_id`,`dict_balance_change_type`.`name` AS `balance_change_type_name` from (`sys_user_balance_change` left join `dict_balance_change_type` on((`dict_balance_change_type`.`id` = `sys_user_balance_change`.`balance_change_type_id`)));

-- ----------------------------
-- View structure for sys_user_view
-- ----------------------------
DROP VIEW IF EXISTS `sys_user_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `sys_user_view` AS select `sys_user`.`id` AS `id`,`sys_user`.`login_name` AS `login_name`,`sys_user`.`login_pwd` AS `login_pwd`,`sys_user`.`login_pwd_guid` AS `login_pwd_guid`,`sys_user`.`role_id` AS `role_id`,`sys_user`.`name` AS `name`,`sys_user`.`phone` AS `phone`,`sys_user`.`address` AS `address`,`sys_user`.`email` AS `email`,`sys_user`.`qq` AS `qq`,`sys_user`.`weixin` AS `weixin`,`sys_user`.`balance` AS `balance`,`sys_user`.`reference_sys_user_id` AS `reference_sys_user_id`,`sys_user`.`register_date` AS `register_date`,`sys_user`.`last_login_date` AS `last_login_date`,`sys_user`.`last_login_ip` AS `last_login_ip`,`sys_user`.`delete_flag` AS `delete_flag`,`dict_role`.`name` AS `role_name`,`reference_sys_user`.`login_name` AS `reference_sys_user_login_name` from ((`sys_user` left join `dict_role` on((`dict_role`.`id` = `sys_user`.`role_id`))) left join `sys_user` `reference_sys_user` on((`reference_sys_user`.`id` = `sys_user`.`reference_sys_user_id`)));

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `check_in` VALUES ('1', '1', '2014-08-11');
INSERT INTO `dict_balance_change_type` VALUES ('1', '推荐好友');
INSERT INTO `dict_balance_change_type` VALUES ('2', '赛事接龙');
INSERT INTO `dict_balance_change_type` VALUES ('3', '赛事结果竞猜');
INSERT INTO `dict_balance_change_type` VALUES ('4', '礼品兑换');
INSERT INTO `dict_balance_change_type` VALUES ('5', '每日签到');
INSERT INTO `dict_balance_change_type` VALUES ('6', '礼品竞拍');
INSERT INTO `dict_balance_change_type` VALUES ('7', '赛事比分竞猜');
INSERT INTO `dict_chain_vote_status` VALUES ('1', '进行中');
INSERT INTO `dict_chain_vote_status` VALUES ('2', '成功');
INSERT INTO `dict_chain_vote_status` VALUES ('3', '失败');
INSERT INTO `dict_football_team` VALUES ('1', '2rr', '2', null);
INSERT INTO `dict_football_tournament` VALUES ('1', '中超');
INSERT INTO `dict_football_tournament` VALUES ('2', '亚冠');
INSERT INTO `dict_football_tournament` VALUES ('3', '足协杯');
INSERT INTO `dict_gift_bid_status` VALUES ('1', '进行中');
INSERT INTO `dict_gift_bid_status` VALUES ('2', '成功');
INSERT INTO `dict_gift_bid_status` VALUES ('3', '失败');
INSERT INTO `dict_gift_bid_status` VALUES ('4', '已退款');
INSERT INTO `dict_match_result` VALUES ('1', '主赢');
INSERT INTO `dict_match_result` VALUES ('2', '客赢');
INSERT INTO `dict_match_result` VALUES ('3', '平局');
INSERT INTO `dict_match_score_type` VALUES ('1', '单数');
INSERT INTO `dict_match_score_type` VALUES ('2', '双数');
INSERT INTO `dict_role` VALUES ('1', '管理员');
INSERT INTO `dict_role` VALUES ('2', '会员');
INSERT INTO `football_match` VALUES ('1', '1', '2011-01-01 00:00:00', '1', '1', '2015-01-01 00:00:00', '2011-01-01 00:00:00', '1', '2', '1', '2', '1');
INSERT INTO `football_match` VALUES ('2', '1', '2011-01-01 00:00:00', '1', '1', '2015-01-01 00:00:00', null, null, null, null, null, '1');
INSERT INTO `football_match` VALUES ('3', '1', '2011-01-01 00:00:00', '1', '1', '2015-01-01 00:00:00', null, null, null, null, null, '1');
INSERT INTO `football_match_score` VALUES ('1', '1', '1', '1', '2', '2014-08-11 21:29:02');
INSERT INTO `football_match_score` VALUES ('2', '1', '2', '1', '2', '2014-08-11 21:29:45');
INSERT INTO `football_match_score` VALUES ('3', '1', '3', '2', '5', '2014-08-11 21:30:00');
INSERT INTO `football_match_vote` VALUES ('1', '1', '1', '1', '2', '2014-08-11 21:31:01');
INSERT INTO `football_match_vote` VALUES ('2', '1', '2', '2', '5', '2014-08-11 21:32:13');
INSERT INTO `football_match_vote` VALUES ('3', '1', '3', '1', '4', '2014-08-11 21:35:32');
INSERT INTO `football_round` VALUES ('1', '1', '2011', 'sdfeff', '2011-01-01 00:00:00', '2015-01-01 00:00:00');
INSERT INTO `football_round_chain` VALUES ('1', '1', 'zzz', '3', null, '100', '2015-01-01 00:00:00', '1', null);
INSERT INTO `football_round_chain_vote` VALUES ('1', '1', '1', '0', '2', '2014-08-11 22:13:57');
INSERT INTO `football_round_chain_vote_invite` VALUES ('1', '1', '1', '1', '1', '2014-08-11 22:13:57');
INSERT INTO `football_round_chain_vote_invite` VALUES ('2', '1', '2', '2', '1', '2014-08-11 22:14:43');
INSERT INTO `football_round_chain_vote_invite` VALUES ('3', '1', '3', '3', '1', '2014-08-11 22:14:49');
INSERT INTO `gift` VALUES ('1', '1', '2', '3', '3', '2014-11-05 00:00:00', '6', null);
INSERT INTO `gift_auction` VALUES ('1', '1', '11', '22', '1', '2011-01-01 00:00:00', '2014-08-11 21:03:37', null);
INSERT INTO `gift_auction_sys_user_bid` VALUES ('1', '1', '1', '1', '4', '2014-08-11 20:59:46');
INSERT INTO `gift_auction_sys_user_bid` VALUES ('2', '1', '2', '3', '2', '2014-08-11 21:01:02');
INSERT INTO `gift_history` VALUES ('1', '1', '1', '2014-08-11 20:56:18');
INSERT INTO `sys_user` VALUES ('1', 'admin', 'c11ffccaf5ca7af075d426e316cff369', 'f5882dbd-dc70-4925-b33a-513c2fe687e5', '1', '11', '22', '33', '44', '55', '66', '202.53', null, '2014-08-11 09:40:00', '2014-08-11 09:47:12', '::1', null);
INSERT INTO `sys_user` VALUES ('2', 'admin1', 'd2397f978016d96d66f9fa51b5d791fc', '9ecf6a51-ff11-46c0-b6a0-628b8b0c2640', '2', null, null, null, null, null, null, '196.60', '2', '2014-08-11 09:43:33', null, null, null);
INSERT INTO `sys_user` VALUES ('3', 'admin1w', '37084bddacc4a0f8190929226a78fe66', '7e03e4f3-6984-4ee4-8526-14e4be90d9b6', '2', null, null, null, null, null, null, '196.87', '1', '2014-08-11 09:44:17', null, null, null);
INSERT INTO `sys_user_balance_change` VALUES ('1', '1', '5', '1.00', '2014-08-11 20:48:17', '签到赢1爱心', null, null);
INSERT INTO `sys_user_balance_change` VALUES ('2', '1', '4', '-3.00', '2014-08-11 20:56:18', '兑换“1”', 'gift', '1');
INSERT INTO `sys_user_balance_change` VALUES ('3', '2', '6', '-3.00', '2014-08-11 21:03:37', '竞拍“11”', 'gift_auction_sys_user_bid', '2');
INSERT INTO `sys_user_balance_change` VALUES ('4', '1', '7', '-1.00', '2014-08-11 21:19:24', 'sdfeff 2rrVS2rr 单数', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('5', '1', '7', '0.00', '2014-08-11 21:19:43', 'sdfeff 2rrVS2rr 单数', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('6', '1', '7', '0.00', '2014-08-11 21:20:29', 'sdfeff 2rrVS2rr 单数', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('7', '1', '7', '0.00', '2014-08-11 21:21:29', 'sdfeff 2rrVS2rr 单数', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('8', '1', '7', '-1.00', '2014-08-11 21:29:03', 'sdfeff 2rrVS2rr 单数', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('9', '2', '7', '-2.00', '2014-08-11 21:29:45', 'sdfeff 2rrVS2rr 单数', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('10', '3', '7', '-5.00', '2014-08-11 21:30:00', 'sdfeff 2rrVS2rr 双数', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('11', '1', '3', '-1.00', '2014-08-11 21:30:30', 'sdfeff 2rrVS2rr 主赢', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('12', '1', '3', '-1.00', '2014-08-11 21:31:01', 'sdfeff 2rrVS2rr 主赢', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('13', '2', '3', '-4.00', '2014-08-11 21:32:04', 'sdfeff 2rrVS2rr 主赢', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('14', '2', '3', '-1.00', '2014-08-11 21:32:13', 'sdfeff 2rrVS2rr 客赢', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('15', '3', '3', '-4.00', '2014-08-11 21:35:33', 'sdfeff 2rrVS2rr 主赢', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('16', '1', '3', '2.93', '2014-08-11 21:39:01', 'sdfeff 2rrVS2rr 主赢', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('17', '3', '3', '5.87', '2014-08-11 21:39:04', 'sdfeff 2rrVS2rr 主赢', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('18', '1', '7', '3.60', '2014-08-11 21:39:08', 'sdfeff 2rrVS2rr 单数', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('19', '2', '7', '3.60', '2014-08-11 21:39:09', 'sdfeff 2rrVS2rr 单数', 'football_match', '1');
INSERT INTO `sys_user_balance_change` VALUES ('20', '1', '2', '100.00', '2014-08-11 22:28:00', 'sdfeff zzz', 'football_round_chain_vote', '1');
INSERT INTO `sys_user_balance_change` VALUES ('21', '2', '2', '100.00', '2014-08-11 22:28:03', 'sdfeff zzz', 'football_round_chain_vote', '1');
INSERT INTO `sys_user_balance_change` VALUES ('22', '3', '2', '100.00', '2014-08-11 22:28:08', 'sdfeff zzz', 'football_round_chain_vote', '1');
