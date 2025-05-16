import React from "react";
import { View, Text, Button, TextInput } from "react-native";
import { NativeStackScreenProps } from "@react-navigation/native-stack";
import { RootStackParamList } from "./types";
import { ItemContext } from "./context";
import { ItemContextType, IItem } from "./types";
import { useContext, useState } from "react";

type EditScreenProps = NativeStackScreenProps<RootStackParamList, "Edit">;

export default function EditScreen({ navigation, route }: EditScreenProps) {
  const context = useContext(ItemContext);

 const { item } = route.params;
  const [title, setTitle] = useState(item?.title ?? "");
  const handleSubmit = () => {


    
    if (title.trim()) {
      const newBulletin = { id: Date.now().toString(), title: title.trim() };

      if (!context) {
        alert("Context not avaialable");
        return;
      }

      const { saveBulletins } = context;

      const updatedBulletin = {
        id: item?.id??Date.now().toString(),
        title: title.trim(),
      }

      saveBulletins(updatedBulletin);

      navigation.navigate("MemberBulletinSummary");
    } else {
      alert("Please fill out all fields.");
    }
  };

  if (!context) {
    return <Text> Loading....</Text>;
  }
  const { bulletins, deleteBulletin } = context;
 

  const deleteItem = (idToDelete: string) => {
    deleteBulletin(idToDelete);
    navigation.navigate("MemberBulletinSummary");
  };

  return (
    <View>
      <Text>Member bulletin details</Text>

      <Text>{item.id}</Text>
      <Text>Title</Text>
      <TextInput value = {title} onChangeText = {setTitle}></TextInput>

      <Button
        title="Submit"
        onPress={() => {
          handleSubmit();
        }}
      />

      <Button
        title="Delete"
        onPress={() => {
          deleteItem(item.id);
        }}
      />
    </View>
  );
}
