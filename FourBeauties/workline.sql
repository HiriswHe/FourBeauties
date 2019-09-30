/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50643
Source Host           : localhost:3306
Source Database       : mt_produce

Target Server Type    : MYSQL
Target Server Version : 50643
File Encoding         : 65001

Date: 2019-09-30 20:15:22
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for workline
-- ----------------------------
DROP TABLE IF EXISTS `workline`;
CREATE TABLE `workline` (
  `workline_uuid` varchar(32) NOT NULL,
  `factory_code` varchar(32) DEFAULT NULL,
  `workshop_code` varchar(32) DEFAULT NULL,
  `workline_code` varchar(32) NOT NULL,
  `workline_name` varchar(32) NOT NULL,
  `enterprise_code` varchar(32) DEFAULT NULL,
  `create_time` datetime NOT NULL,
  `update_time` datetime DEFAULT NULL,
  `workshop_uuid` varchar(32) DEFAULT NULL COMMENT '车间UUID',
  `factory_uuid` varchar(32) DEFAULT NULL COMMENT '工厂UUID',
  PRIMARY KEY (`workline_uuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
