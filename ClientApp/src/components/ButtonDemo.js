import React from 'react';
import CustomButton from './CustomButton';

// Demo icons (bạn có thể thay thế bằng icons thực tế)
const HomeIcon = () => (
  <svg width="24" height="24" viewBox="0 0 24 24" fill="currentColor">
    <path d="M10 20v-6h4v6h5v-8h3L12 3 2 12h3v8z"/>
  </svg>
);

const UserIcon = () => (
  <svg width="24" height="24" viewBox="0 0 24 24" fill="currentColor">
    <path d="M12 12c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm0 2c-2.67 0-8 1.34-8 4v2h16v-2c0-2.66-5.33-4-8-4z"/>
  </svg>
);

const SettingsIcon = () => (
  <svg width="24" height="24" viewBox="0 0 24 24" fill="currentColor">
    <path d="M19.14,12.94c0.04-0.3,0.06-0.61,0.06-0.94c0-0.32-0.02-0.64-0.07-0.94l2.03-1.58c0.18-0.14,0.23-0.41,0.12-0.61 l-1.92-3.32c-0.12-0.22-0.37-0.29-0.59-0.22l-2.39,0.96c-0.5-0.38-1.03-0.7-1.62-0.94L14.4,2.81c-0.04-0.24-0.24-0.41-0.48-0.41 h-3.84c-0.24,0-0.43,0.17-0.47,0.41L9.25,5.35C8.66,5.59,8.12,5.92,7.63,6.29L5.24,5.33c-0.22-0.08-0.47,0-0.59,0.22L2.74,8.87 C2.62,9.08,2.66,9.34,2.86,9.48l2.03,1.58C4.84,11.36,4.8,11.69,4.8,12s0.02,0.64,0.07,0.94l-2.03,1.58 c-0.18,0.14-0.23,0.41-0.12,0.61l1.92,3.32c0.12,0.22,0.37,0.29,0.59,0.22l2.39-0.96c0.5,0.38,1.03,0.7,1.62,0.94l0.36,2.54 c0.05,0.24,0.24,0.41,0.48,0.41h3.84c0.24,0,0.44-0.17,0.47-0.41l0.36-2.54c0.59-0.24,1.13-0.56,1.62-0.94l2.39,0.96 c0.22,0.08,0.47,0,0.59-0.22l1.92-3.32c0.12-0.22,0.07-0.47-0.12-0.61L19.14,12.94z M12,15.6c-1.98,0-3.6-1.62-3.6-3.6 s1.62-3.6,3.6-3.6s3.6,1.62,3.6,3.6S13.98,15.6,12,15.6z"/>
  </svg>
);

const ButtonDemo = () => {
  return (
    <div style={{ 
      padding: '20px', 
      backgroundColor: '#f8f9fa', 
      minHeight: '100vh',
      fontFamily: 'system-ui, -apple-system, sans-serif'
    }}>
      <h1 style={{ textAlign: 'center', marginBottom: '40px', color: '#333' }}>
        🎨 Custom Button Component Demo
      </h1>

      {/* Basic Examples */}
      <section style={{ marginBottom: '40px' }}>
        <h2 style={{ marginBottom: '20px', color: '#555' }}>Các ví dụ cơ bản</h2>
        <div style={{ 
          display: 'grid', 
          gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', 
          gap: '20px',
          marginBottom: '20px'
        }}>
          <CustomButton
            icon={<HomeIcon />}
            label="Trang chủ"
            onClick={() => alert('Bạn đã click vào Trang chủ!')}
          />
          
          <CustomButton
            image="https://via.placeholder.com/24x24/3498db/ffffff?text=📊"
            label="Báo cáo"
            description="Xem báo cáo chi tiết"
            onClick={() => alert('Bạn đã click vào Báo cáo!')}
          />
          
          <CustomButton
            icon={<UserIcon />}
            label="Hồ sơ"
            variant="secondary"
            onClick={() => alert('Bạn đã click vào Hồ sơ!')}
          />
        </div>
      </section>

      {/* Size Variants */}
      <section style={{ marginBottom: '40px' }}>
        <h2 style={{ marginBottom: '20px', color: '#555' }}>Các kích thước khác nhau</h2>
        <div style={{ 
          display: 'flex', 
          flexWrap: 'wrap', 
          gap: '15px', 
          alignItems: 'center',
          marginBottom: '20px'
        }}>
          <CustomButton
            icon={<SettingsIcon />}
            label="Nhỏ"
            size="small"
            variant="info"
            onClick={() => alert('Button nhỏ!')}
          />
          
          <CustomButton
            icon={<SettingsIcon />}
            label="Trung bình"
            size="medium"
            variant="info"
            onClick={() => alert('Button trung bình!')}
          />
          
          <CustomButton
            icon={<SettingsIcon />}
            label="Lớn"
            size="large"
            variant="info"
            onClick={() => alert('Button lớn!')}
          />
          
          <CustomButton
            icon={<SettingsIcon />}
            label="Rất lớn"
            size="xlarge"
            variant="info"
            onClick={() => alert('Button rất lớn!')}
          />
        </div>
      </section>

      {/* Color Variants */}
      <section style={{ marginBottom: '40px' }}>
        <h2 style={{ marginBottom: '20px', color: '#555' }}>Các màu sắc khác nhau</h2>
        <div style={{ 
          display: 'grid', 
          gridTemplateColumns: 'repeat(auto-fit, minmax(150px, 1fr))', 
          gap: '15px',
          marginBottom: '20px'
        }}>
          <CustomButton
            icon={<HomeIcon />}
            label="Primary"
            variant="primary"
            onClick={() => alert('Primary button!')}
          />
          
          <CustomButton
            icon={<UserIcon />}
            label="Success"
            variant="success"
            onClick={() => alert('Success button!')}
          />
          
          <CustomButton
            icon={<SettingsIcon />}
            label="Warning"
            variant="warning"
            onClick={() => alert('Warning button!')}
          />
          
          <CustomButton
            icon={<HomeIcon />}
            label="Danger"
            variant="danger"
            onClick={() => alert('Danger button!')}
          />
          
          <CustomButton
            icon={<UserIcon />}
            label="Info"
            variant="info"
            onClick={() => alert('Info button!')}
          />
          
          <CustomButton
            icon={<SettingsIcon />}
            label="Secondary"
            variant="secondary"
            onClick={() => alert('Secondary button!')}
          />
        </div>
      </section>

      {/* Shape Variants */}
      <section style={{ marginBottom: '40px' }}>
        <h2 style={{ marginBottom: '20px', color: '#555' }}>Các hình dạng khác nhau</h2>
        <div style={{ 
          display: 'flex', 
          flexWrap: 'wrap', 
          gap: '15px', 
          alignItems: 'center',
          marginBottom: '20px'
        }}>
          <CustomButton
            icon={<HomeIcon />}
            label="Rounded"
            shape="rounded"
            variant="primary"
            onClick={() => alert('Rounded button!')}
          />
          
          <CustomButton
            icon={<UserIcon />}
            label="Circle"
            shape="circle"
            variant="success"
            onClick={() => alert('Circle button!')}
          />
          
          <CustomButton
            icon={<SettingsIcon />}
            label="Square"
            shape="square"
            variant="warning"
            onClick={() => alert('Square button!')}
          />
        </div>
      </section>

      {/* Animation Variants */}
      <section style={{ marginBottom: '40px' }}>
        <h2 style={{ marginBottom: '20px', color: '#555' }}>Các hiệu ứng animation</h2>
        <div style={{ 
          display: 'flex', 
          flexWrap: 'wrap', 
          gap: '15px', 
          alignItems: 'center',
          marginBottom: '20px'
        }}>
          <CustomButton
            icon={<HomeIcon />}
            label="Scale"
            animation="scale"
            variant="primary"
            onClick={() => alert('Scale animation!')}
          />
          
          <CustomButton
            icon={<UserIcon />}
            label="Bounce"
            animation="bounce"
            variant="success"
            onClick={() => alert('Bounce animation!')}
          />
          
          <CustomButton
            icon={<SettingsIcon />}
            label="Pulse"
            animation="pulse"
            variant="warning"
            onClick={() => alert('Pulse animation!')}
          />
        </div>
      </section>

      {/* Direction Variants */}
      <section style={{ marginBottom: '40px' }}>
        <h2 style={{ marginBottom: '20px', color: '#555' }}>Hướng layout</h2>
        <div style={{ 
          display: 'flex', 
          flexWrap: 'wrap', 
          gap: '15px', 
          alignItems: 'center',
          marginBottom: '20px'
        }}>
          <CustomButton
            icon={<HomeIcon />}
            label="Vertical"
            direction="vertical"
            variant="primary"
            onClick={() => alert('Vertical layout!')}
          />
          
          <CustomButton
            icon={<UserIcon />}
            label="Horizontal"
            direction="horizontal"
            variant="success"
            onClick={() => alert('Horizontal layout!')}
          />
        </div>
      </section>

      {/* Special States */}
      <section style={{ marginBottom: '40px' }}>
        <h2 style={{ marginBottom: '20px', color: '#555' }}>Các trạng thái đặc biệt</h2>
        <div style={{ 
          display: 'flex', 
          flexWrap: 'wrap', 
          gap: '15px', 
          alignItems: 'center',
          marginBottom: '20px'
        }}>
          <CustomButton
            icon={<HomeIcon />}
            label="Bình thường"
            variant="primary"
            onClick={() => alert('Button bình thường!')}
          />
          
          <CustomButton
            icon={<UserIcon />}
            label="Đang tải"
            loading={true}
            variant="success"
            onClick={() => alert('Button đang tải!')}
          />
          
          <CustomButton
            icon={<SettingsIcon />}
            label="Vô hiệu hóa"
            disabled={true}
            variant="secondary"
            onClick={() => alert('Button bị vô hiệu hóa!')}
          />
        </div>
      </section>

      {/* Full Width Example */}
      <section style={{ marginBottom: '40px' }}>
        <h2 style={{ marginBottom: '20px', color: '#555' }}>Button toàn chiều rộng</h2>
        <div style={{ marginBottom: '20px' }}>
          <CustomButton
            icon={<HomeIcon />}
            label="Button toàn chiều rộng"
            description="Click để xem hiệu ứng"
            fullWidth={true}
            variant="primary"
            onClick={() => alert('Button toàn chiều rộng!')}
          />
        </div>
      </section>

      {/* Custom Colors */}
      <section style={{ marginBottom: '40px' }}>
        <h2 style={{ marginBottom: '20px', color: '#555' }}>Màu sắc tùy chỉnh</h2>
        <div style={{ 
          display: 'flex', 
          flexWrap: 'wrap', 
          gap: '15px', 
          alignItems: 'center',
          marginBottom: '20px'
        }}>
          <CustomButton
            icon={<HomeIcon />}
            label="Màu tím"
            bgColor="#9b59b6"
            hoverColor="#8e44ad"
            onClick={() => alert('Button màu tím!')}
          />
          
          <CustomButton
            icon={<UserIcon />}
            label="Màu cam"
            bgColor="#e67e22"
            hoverColor="#d35400"
            onClick={() => alert('Button màu cam!')}
          />
          
          <CustomButton
            icon={<SettingsIcon />}
            label="Màu hồng"
            bgColor="#e91e63"
            hoverColor="#c2185b"
            onClick={() => alert('Button màu hồng!')}
          />
        </div>
      </section>

      {/* Usage Instructions */}
      <section style={{ 
        backgroundColor: '#fff', 
        padding: '20px', 
        borderRadius: '8px', 
        boxShadow: '0 2px 8px rgba(0,0,0,0.1)',
        marginTop: '40px'
      }}>
        <h2 style={{ marginBottom: '15px', color: '#333' }}>📖 Cách sử dụng</h2>
        <div style={{ marginBottom: '15px' }}>
          <h3 style={{ color: '#555', marginBottom: '8px' }}>Import component:</h3>
          <pre style={{ 
            backgroundColor: '#f8f9fa', 
            padding: '10px', 
            borderRadius: '4px', 
            fontSize: '14px',
            overflow: 'auto'
          }}>
{`import CustomButton from './components/CustomButton';`}
          </pre>
        </div>
        
        <div style={{ marginBottom: '15px' }}>
          <h3 style={{ color: '#555', marginBottom: '8px' }}>Sử dụng cơ bản:</h3>
          <pre style={{ 
            backgroundColor: '#f8f9fa', 
            padding: '10px', 
            borderRadius: '4px', 
            fontSize: '14px',
            overflow: 'auto'
          }}>
{`<CustomButton
  icon={<HomeIcon />}
  label="Trang chủ"
  onClick={() => console.log('Clicked!')}
/>`}
          </pre>
        </div>
        
        <div>
          <h3 style={{ color: '#555', marginBottom: '8px' }}>Các props chính:</h3>
          <ul style={{ paddingLeft: '20px', lineHeight: '1.6' }}>
            <li><strong>icon/image:</strong> Icon hoặc hình ảnh</li>
            <li><strong>label:</strong> Nhãn chính</li>
            <li><strong>description:</strong> Mô tả phụ</li>
            <li><strong>variant:</strong> primary, secondary, success, warning, danger, info</li>
            <li><strong>size:</strong> small, medium, large, xlarge</li>
            <li><strong>shape:</strong> rounded, circle, square</li>
            <li><strong>animation:</strong> scale, bounce, pulse, none</li>
            <li><strong>direction:</strong> vertical, horizontal</li>
            <li><strong>disabled/loading:</strong> Trạng thái đặc biệt</li>
          </ul>
        </div>
      </section>
    </div>
  );
};

export default ButtonDemo;
