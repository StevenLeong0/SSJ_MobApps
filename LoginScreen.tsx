import React, {useState} from 'react';
import { View, Text, TextInput, Button} from 'react-native';
import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from './types';

type LoginScreenProps = NativeStackScreenProps<RootStackParamList, 'Login'>;





export default function LoginScreen({ navigation }: LoginScreenProps) {


      const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

const handleSubmit = () => {
  // Assuming you have form fields like 'username', 'email', etc.
  if (username.trim() && password.trim()) {
    // Update the state or perform any checks
    // Navigate and pass form data to the next screen
    navigation.navigate('BulletinChoice', { username, password });
  } else {
    alert('Please fill out all fields.');
  }
};


  return (



    
    <View>
      <Text>Login Screen</Text>

 <TextInput
        
        placeholder="Enter username"
        onChangeText={newText => setUsername(newText)}
      
      />

 <TextInput
        
        placeholder="Enter password"
        onChangeText={newText => setPassword(newText)}
      
      />

      <Button
        title="Submit"
        onPress= {handleSubmit}
      />

    <Button
        title="Register"
        onPress={() => navigation.navigate('Register')}
      />

    </View>
  );
}




