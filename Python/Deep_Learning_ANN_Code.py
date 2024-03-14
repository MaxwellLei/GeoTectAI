import pandas as pd
import torch
import torch.nn as nn
import torch.nn.functional as F
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler, LabelEncoder

def load_data(filename):
    data = pd.read_excel(filename)
    X = data.iloc[:, 1:]  # 所有行，除了最后一列
    y = data.iloc[:, 0]  # 所有行，只有最后一列
    scaler = StandardScaler()
    X = scaler.fit_transform(X)
    encoder = LabelEncoder()
    y = encoder.fit_transform(y)
    return train_test_split(X, y, test_size=0.2, random_state=42)

class Net(nn.Module):
    def __init__(self):
        super(Net, self).__init__()
        self.fc1 = nn.Linear(23, 64)
        self.fc2 = nn.Linear(64, 128)
        self.fc3 = nn.Linear(128, 64)
        self.fc4 = nn.Linear(64, 6)

    def forward(self, x):
        x = F.relu(self.fc1(x))
        x = F.relu(self.fc2(x))
        x = F.relu(self.fc3(x))
        x = self.fc4(x)
        return x


def train_model(X_train, y_train):
    model = Net()
    criterion = nn.CrossEntropyLoss()
    optimizer = torch.optim.Adam(model.parameters(), lr=0.001)
    epochs = 2000

    for epoch in range(epochs):
        inputs = torch.tensor(X_train, dtype=torch.float)
        labels = torch.tensor(y_train, dtype=torch.long)
        outputs = model(inputs)
        loss = criterion(outputs, labels)

        optimizer.zero_grad()
        loss.backward()
        optimizer.step()

        if (epoch + 1) % 10 == 0:
            print(f'Epoch [{epoch + 1}/{epochs}], Loss: {loss.item():.4f}')

    return model


def evaluate_model(model, X_test, y_test):
    with torch.no_grad():
        predicted = model(torch.tensor(X_test, dtype=torch.float)).argmax(1)
        accuracy = (predicted == torch.tensor(y_test)).float().mean()
        print(f'Accuracy: {accuracy:.4f}')


def convert_to_onnx(model, input_shape):
    model.eval()
    dummy_input = torch.randn(input_shape, requires_grad=True)
    onnx_file_path = 'ann_model.onnx'
    torch.onnx.export(model, dummy_input, onnx_file_path, export_params=True)
    print(f"Model exported to {onnx_file_path}")


def main():
    X_train, X_test, y_train, y_test = load_data('new_test_data.xlsx')
    model = train_model(X_train, y_train)
    evaluate_model(model, X_test, y_test)
    convert_to_onnx(model, (1, 23))


if __name__ == "__main__":
    main()
