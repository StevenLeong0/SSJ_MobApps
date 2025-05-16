import React from "react";
import { View, Text, Button } from "react-native";
import { NativeStackScreenProps } from "@react-navigation/native-stack";
import { RootStackParamList } from "./types";

type BulletinChoiceScreenProps = NativeStackScreenProps<
  RootStackParamList,
  "BulletinChoice"
>;

export default function BulletinChoiceScreen({
  navigation,
}: BulletinChoiceScreenProps) {
  return (
    <View>
      <Text>Bulletin Choice Screen</Text>
      <Button
        title="Go to member bulletins"
        onPress={() => navigation.navigate("MemberBulletinSummary")}
      />

      {/*    <Button
        title="Go to ofiical bulletins"
        onPress={() => navigation.navigate("OfficialBulletinSummary")}
      /> */}
    </View>
  );
}
