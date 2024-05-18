import pandas as pd
import torch
import torch.nn as nn
import torch.nn.functional as F
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler, LabelEncoder
import numpy as np


# 数据预处理
def load_data(filename):
    data = pd.read_excel(filename)
    X = data.iloc[:, 7:]
    y = data.iloc[:, 6]
    features_dropped = X.dropna()
    label_dropped = y[features_dropped.index]

    Q1 = np.percentile(features_dropped, 25, axis=0)
    Q3 = np.percentile(features_dropped, 75, axis=0)
    IQR = Q3 - Q1

    mask_all = np.ones(len(features_dropped), dtype=bool)
    for i in range(features_dropped.shape[1]):
        feature = features_dropped.iloc[:, i]
        mask_feature = (feature >= (Q1[i] - 1.5 * IQR[i])) & (feature <= (Q3[i] + 1.5 * IQR[i]))
        mask_all = mask_all & mask_feature

    features_filtered = features_dropped[mask_all] 
    label_filtered = label_dropped[mask_all]
    scaler = StandardScaler()
    X = scaler.fit_transform(features_filtered)
    encoder = LabelEncoder()
    y = encoder.fit_transform(label_filtered)

    class_mapping = dict(zip(encoder.classes_, encoder.transform(encoder.classes_)))
    print("Class Mapping: ", class_mapping)
    return train_test_split(X, y, test_size=0.2, random_state=42)


# 模型定义
class Net(nn.Module):
    def __init__(self):
        super(Net, self).__init__()
        self.fc1 = nn.Linear(23, 64)
        self.fc2 = nn.Linear(64, 128)
        self.fc_out = nn.Linear(128, 7)

    def forward(self, x):
        x = F.relu(self.fc1(x))
        x = F.relu(self.fc2(x))
        x = self.fc_out(x)
        return x


# 训练模型
def train_model(X_train, y_train):
    model = Net()
    criterion = nn.CrossEntropyLoss()
    optimizer = torch.optim.Adam(model.parameters(), lr=0.001)
    epochs = 8000

    for epoch in range(epochs):
        inputs = torch.tensor(X_train, dtype=torch.float)
        labels = torch.tensor(y_train, dtype=torch.long)
        outputs = model(inputs)
        loss = criterion(outputs, labels)

        optimizer.zero_grad()
        loss.backward()
        optimizer.step()

        if (epoch + 1) % 100 == 0:
            print(f'Epoch [{epoch + 1}/{epochs}], Loss: {loss.item():.4f}')

    return model


# 转换模型为 ONNX
def convert_to_onnx(model, input_shape):

    model.eval()

    dummy_input = torch.randn(input_shape, requires_grad=True)

    onnx_file_path = 'ann_model_.onnx'

    torch.onnx.export(model, dummy_input, onnx_file_path, export_params=True)

    print(f"Model exported to {onnx_file_path}")

def main():
    X_train, X_test, y_train, y_test = load_data('Database_7_AKA.xlsx')
    model = train_model(X_train, y_train)

    convert_to_onnx(model, (1, 23))


if __name__ == "__main__":
    main()